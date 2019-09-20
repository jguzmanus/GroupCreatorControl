using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GroupCreatorControl
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      
      public MainWindow()
      {
         InitializeComponent();

         
         var ctx = new GroupCreatorContext(GetExistingGroups(), GetAllFacilities())
        // var ctx = new GroupCreatorContext(new Dictionary<string, Group>(), GetAllFacilities())
         {
            AvailableText = "Available Facilities", AssignedText = "Assigned Facilities"
         };


         var dlg = new GroupCreatorCtl
         {
            Context = ctx
         };

         var res = dlg.ShowDialog();

      
      }

      public IEnumerable<IItemPresenter> GetAllFacilities()
      {

         return Enumerable.Range(0, 10).Select(item => new ItemPresenter {ID = item, Value = $"Facility {item}"});
      }

      public Dictionary<string, Group> GetExistingGroups()
      {
         var groups = new[]
         {
            new Group { Name = "Group 1", AssignedItems = new List<IItemPresenter>
            {
               new ItemPresenter() {ID = 0, Value = "Facility 0"},
               new ItemPresenter() {ID = 1, Value = "Facility 1"},
               new ItemPresenter() {ID = 3, Value = "Facility 3"},
            }},
            new Group { Name = "Group 2", AssignedItems = new List<IItemPresenter>
            {
               new ItemPresenter() {ID = 2, Value = "Facility 2"},
               new ItemPresenter() {ID = 4, Value = "Facility 4"},
               new ItemPresenter() {ID = 5, Value = "Facility 5"},
            }},
         };

         return groups.ToDictionary(k => k.Name, g => g);
      }
   }

  
}
