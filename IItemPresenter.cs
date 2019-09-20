using System;
using System.Windows.Input;

namespace GroupCreatorControl
{
   public interface IItemPresenter : IComparable
   {
      int ID { get; set; }
      object Value { get; set; }
      
   }
}
