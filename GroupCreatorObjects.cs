using System;
using System.Collections.Generic;

namespace GroupCreatorControl
{
   public class Group
   {
      public string Name;
      public List<IItemPresenter> AssignedItems;
   }

   public class ItemPresenter : IItemPresenter
   {
      public int ID { get; set; }
      public object Value { get; set; }
      public int CompareTo(object obj)
      {
         var other = obj as IItemPresenter;
         if (obj == null)
            return 1;
         if (other != null)
            return ID.CompareTo(other.ID);

         throw new ArgumentException("Object is does not implement IItemPresenter");
      }
   }
   
}
