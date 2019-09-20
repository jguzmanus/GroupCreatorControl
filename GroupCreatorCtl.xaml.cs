using System.Windows;
using System.Windows.Controls;

namespace GroupCreatorControl
{
   /// <summary>
   /// Interaction logic for GroupCreatorCtl.xaml
   /// </summary>
   public partial class GroupCreatorCtl : Window
   {
      public static readonly DependencyProperty ContextProperty = DependencyProperty.Register(
         "Context", typeof(object), typeof(GroupCreatorCtl), new PropertyMetadata(default(IGroupCreatorContext)));

   
    public IGroupCreatorContext Context
      {
         get => (IGroupCreatorContext) GetValue(ContextProperty);
         set => SetValue(ContextProperty, value);
      }
      public GroupCreatorCtl()
      {
         InitializeComponent();
      }

      private void SaveClick(object sender, RoutedEventArgs e)
      {
         DialogResult = true;
      }

      private void CancelClick(object sender, RoutedEventArgs e)
      {
         DialogResult = false;
      }
   }

   public class GroupCreatorTemplateSelector : DataTemplateSelector
   {
      public override DataTemplate SelectTemplate(object item, DependencyObject container) => item is IItemPresenter ? ItemDataTemplate : base.SelectTemplate(item, container);

      public DataTemplate ItemDataTemplate { get; set; }
   }
}
