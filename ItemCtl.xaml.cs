using System.Windows;
using System.Windows.Controls;

namespace GroupCreatorControl
{
   /// <summary>
   /// Interaction logic for ItemCtl.xaml
   /// </summary>
   public partial class ItemCtl : UserControl
   {
      public static readonly DependencyProperty PresenterProperty = DependencyProperty.Register(
         "Presenter", typeof(object), typeof(ItemCtl), new PropertyMetadata(default(IItemPresenter)));

      public IItemPresenter Presenter
      {
         get => (IItemPresenter) GetValue(PresenterProperty);
         set => SetValue(PresenterProperty, value);
      }
      public ItemCtl()
      {
         InitializeComponent();
      }
   }
}
