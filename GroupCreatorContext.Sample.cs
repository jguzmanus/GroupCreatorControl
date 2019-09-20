using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace GroupCreatorControl
{
   public class GroupCreatorContextSample :  IGroupCreatorContext
   {
      public CollectionViewSource AvailableItems { get; set; }
      public CollectionViewSource AssignedItems { get; set; }
      public CollectionViewSource Groups { get; }

      public string AvailableText { get; set; }

      public string AssignedText { get; set; }

      public string SelectedGroup { get; set; }
      public string EnteredText { get; set; }
      public IItemPresenter SelectedAvailableItem { get; set; }
      public IItemPresenter SelectedAssignedItem { get; set; }
      public ICommand AddAvailableItemCommand { get; }
      public ICommand RemoveAssignedItemCommand { get; }
      public ICommand CreateNewGroupCommand { get; }
      public ICommand DeleteGroupCommand { get; }
      public Dictionary<string, Group> UpdatedGroups { get; set; }
      public ICommand SaveCommand { get; }
      

      public GroupCreatorContextSample()
      {
         AvailableText = "Available Items";
         AvailableItems = new CollectionViewSource
         {
            
            Source = new List<string>(
               Enumerable
                  .Range(0,10)
                  .Select(item =>  $"Sample Available Item {item}"))
         };

         AvailableItems.View.Refresh();

         AssignedText = "Assigned Items";
         AssignedItems = new CollectionViewSource
         {

            Source = new List<string>(
               Enumerable
                  .Range(0, 5)
                  .Select(item => $"Sample Assigned Item {item}"))
         };

         AssignedItems.View.Refresh();

         Groups = new CollectionViewSource()
         {
            Source = new List<string>(Enumerable.Range(0,5).Select(item => $"Sample Group {item}"))
         };
         Groups.View.Refresh();
      }
   }
}
