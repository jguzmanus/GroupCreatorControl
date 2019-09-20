using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GroupCreatorControl
{
   public interface IGroupCreatorContext
   {
      CollectionViewSource AvailableItems { get; set; }
      CollectionViewSource AssignedItems { get; set; }
      CollectionViewSource Groups { get; }
      string AvailableText { get; set; }
      string AssignedText { get; set; }
      string SelectedGroup { get; set; }
      string EnteredText { get; set; }
      IItemPresenter SelectedAvailableItem { get; set; }
      IItemPresenter SelectedAssignedItem { get; set; }
      ICommand AddAvailableItemCommand { get; }
      ICommand RemoveAssignedItemCommand { get; }
      ICommand CreateNewGroupCommand { get; }
      ICommand DeleteGroupCommand { get; }
      Dictionary<string, Group> UpdatedGroups { get; set; }
   }
}
