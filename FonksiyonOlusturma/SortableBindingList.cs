using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool isSorted;
        private ListSortDirection sortDirection;
        private PropertyDescriptor sortProperty;

        protected override bool SupportsSortingCore => true;

        protected override bool IsSortedCore => isSorted;

        protected override ListSortDirection SortDirectionCore => sortDirection;

        protected override PropertyDescriptor SortPropertyCore => sortProperty;

        public SortableBindingList(List<T> list) : base(list) { }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            if (prop != null)
            {
                ((List<T>)Items).Sort(new Comparison<T>((x, y) =>
                {
                    var xValue = prop.GetValue(x);
                    var yValue = prop.GetValue(y);
                    return direction == ListSortDirection.Ascending ?
                           Comparer.Default.Compare(xValue, yValue) :
                           Comparer.Default.Compare(yValue, xValue);
                }));
                sortProperty = prop;
                sortDirection = direction;
                isSorted = true;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            sortProperty = null;
        }
    }

}
