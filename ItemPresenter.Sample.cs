namespace GroupCreatorControl
{
   public class ItemPresenterSample :  IItemPresenter
   {
      public int ID
      {
         get;
         set;
      }

      public object Value
      {
         get;
         set;
      }

      
      public ItemPresenterSample()
      {
         ID = 1;
         Value = "Sample Item";
      }

      public int CompareTo(object obj)
      {
         throw new System.NotImplementedException();
      }
   }
}
