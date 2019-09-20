using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using GroupCreatorControl.Annotations;

namespace GroupCreatorControl
{
   public class GroupCreatorContext : IGroupCreatorContext, INotifyPropertyChanged
   {
      public CollectionViewSource AvailableItems { get; set; }
      public CollectionViewSource AssignedItems { get; set; }

      public CollectionViewSource Groups
      {
         get => _groups;
         set
         {
            _groups = value; 
            OnPropertyChanged(nameof(Groups));
         }
      }

      public string AvailableText { get; set; }
      public string AssignedText { get; set; }
      public ICommand AddAvailableItemCommand { get; }
      public ICommand RemoveAssignedItemCommand { get; }
      public ICommand CreateNewGroupCommand { get; }
      public ICommand DeleteGroupCommand { get; }
      public Dictionary<string, Group> UpdatedGroups { get; set; }

      private string _enteredText;
      private IItemPresenter _selectedAssignedItem;
      private string _selectedGroup;
      private readonly IEnumerable<IItemPresenter> _allItems;
      private IEnumerable<IItemPresenter> _availableItems;
      private Group _newGroup;
      private CollectionViewSource _groups;
      private IItemPresenter _selectedAvailableItem;

      public string EnteredText
      {
         get => _enteredText;
         set
         {
            _enteredText = value;
            OnPropertyChanged(nameof(EnteredText));
         }
      }


      public IItemPresenter SelectedAvailableItem
      {
         get => _selectedAvailableItem;
         set
         {
            _selectedAvailableItem = value; 
            OnPropertyChanged(nameof(SelectedAvailableItem));
         }
      }

      public IItemPresenter SelectedAssignedItem
      {
         get => _selectedAssignedItem;
         set
         {
            _selectedAssignedItem = value;
            OnPropertyChanged(nameof(SelectedAssignedItem));
         }
      }
      public string SelectedGroup
      {
         get => _selectedGroup;
         set
         {
            _selectedGroup = value;
            OnPropertyChanged(nameof(SelectedGroup));
            _newGroup = null;

            if (string.IsNullOrEmpty(value))
            {
               MakeAllItemsAvailable();
               return;
            }
            RetrieveGroupInfo();
         }
      }



      private void MakeAllItemsAvailable()
      {
         _availableItems = _allItems;
         AvailableItems.Source = _availableItems;
         AvailableItems.View.Refresh();

         AssignedItems.Source = new IItemPresenter[] { };
         AssignedItems.View.Refresh();
      }

      private void RetrieveGroupInfo()
      {
         AssignedItems.Source = _newGroup == null
            ? UpdatedGroups[SelectedGroup].AssignedItems
            : _newGroup.AssignedItems;
         AssignedItems.View.Refresh();

         RefreshAvailableItems();
      }

      private void RefreshAvailableItems()
      {
         var unassignedItems = _newGroup == null
            ? _allItems.Select(i => i.ID).Except(UpdatedGroups[SelectedGroup].AssignedItems.Select(item => item.ID))
            : _allItems.Select(i => i.ID).Except(_newGroup.AssignedItems.Select(item => item.ID));

         _availableItems = _allItems.Where(i => unassignedItems.Contains(i.ID));
         AvailableItems.Source = _availableItems;
         AvailableItems.View.Refresh();
      }

      public GroupCreatorContext(Dictionary<string, Group> existingGroups, IEnumerable<IItemPresenter> allItems)
      {
         UpdatedGroups = existingGroups;
         _allItems = allItems;
         _availableItems = _allItems;

         Groups = new CollectionViewSource
         {
            Source = UpdatedGroups.Select(g => g.Key)
         };
         Groups.View.Refresh();

         AvailableItems = new CollectionViewSource()
         {
            Source = _availableItems
         };
         AvailableItems.View.Refresh();

         AssignedItems = new CollectionViewSource
         {
            Source = new IItemPresenter[] { }
         };
         AssignedItems.View.Refresh();

         AddAvailableItemCommand = new RelayCommand(AddAvailableItemCmd,
            () => (!string.IsNullOrEmpty(EnteredText) 
                   || !string.IsNullOrEmpty(SelectedGroup)) && SelectedAvailableItem != null);

         RemoveAssignedItemCommand = new RelayCommand(RemoveAssignedItemCmd,
            () => (!string.IsNullOrEmpty(EnteredText) 
                   || !string.IsNullOrEmpty(SelectedGroup)) && SelectedAssignedItem != null);

         CreateNewGroupCommand = new RelayCommand(CreateNewGroupCmd,
            () => _newGroup != null && SelectedAssignedItem != null);

         DeleteGroupCommand = new RelayCommand(DeleteGroupCmd, 
            () => !string.IsNullOrEmpty(SelectedGroup));

      }

     

      private void DeleteGroupCmd()
      {
         UpdatedGroups.Remove(SelectedGroup);
         Groups.View.Refresh();
      }

      private void CreateNewGroupCmd()
      {
         UpdatedGroups.Add(_newGroup.Name, _newGroup);
         SelectedGroup = _newGroup.Name;
         _newGroup = null;
         Groups.View.Refresh();
         RetrieveGroupInfo();
      }

      private void RemoveAssignedItemCmd()
      {
         if (_newGroup != null)
            _newGroup.AssignedItems.Remove(SelectedAssignedItem);
         else
            UpdatedGroups[SelectedGroup].AssignedItems.Remove(SelectedAssignedItem);
         RetrieveGroupInfo();
      }

      private void AddAvailableItemCmd()
      {
         if (SelectedGroup != null)
         {
            UpdatedGroups[SelectedGroup].AssignedItems.Add(SelectedAvailableItem);
            UpdatedGroups[SelectedGroup].AssignedItems.Sort();
            _newGroup = null;
         }
         else if (_newGroup != null)
         {
            _newGroup.AssignedItems.Add(SelectedAvailableItem);
            _newGroup.AssignedItems.Sort();
         }
         else
         {
            _newGroup =
               new Group
               {
                  Name = EnteredText,
                  AssignedItems = new List<IItemPresenter> { SelectedAvailableItem }
               };
         }

         SelectedAssignedItem = SelectedAvailableItem;
         RetrieveGroupInfo();
      }

      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }

   public class RelayCommand : ICommand
   {
      private readonly Action _command;
      private readonly Predicate<object> _canExecute;
      public RelayCommand(Action addAvailableItemCmd, Func<bool> func)
      {

         _command = addAvailableItemCmd;
         _canExecute = o => func();
      }

      public bool CanExecute(object parameter)
      {
         return _canExecute(parameter);
      }

      public void Execute(object parameter)
      {
         _command();
      }

      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (_canExecute != null)
               CommandManager.RequerySuggested += value;
         }
         remove
         {
            if (_canExecute != null)
               CommandManager.RequerySuggested -= value;
         }
      }

   }
}
