using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BorgNetLib
{
    public class SubPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor _subPD;
        private PropertyDescriptor _parentPD;

        public SubPropertyDescriptor(PropertyDescriptor parentPD, PropertyDescriptor subPD, string pdname)
            : base(pdname, null){
            _subPD = subPD;
            _parentPD = parentPD;
        }

        public override bool IsReadOnly { get { return false; } }
        public override void ResetValue(object component) { }
        public override bool CanResetValue(object component) { return false; }
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get { return _parentPD.ComponentType; }
        }

        public override Type PropertyType { get { return _subPD.PropertyType; } }

        public override object GetValue(object component)
        {
            return _subPD.GetValue(_parentPD.GetValue(component));
        }

        public override void SetValue(object component, object value)
        {
            _subPD.SetValue(_parentPD.GetValue(component), value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }

    public class MyCustomTypeDescriptor : CustomTypeDescriptor
    {
        public MyCustomTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)        {       }
        
        public override PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection cols = base.GetProperties();

            PropertyDescriptor addressPD = cols["HomeAddr"];
            PropertyDescriptorCollection homeAddr_child = addressPD.GetChildProperties();
            PropertyDescriptor[] array = new PropertyDescriptor[cols.Count + 2];
            cols.CopyTo(array, 0);
            array[cols.Count] = new SubPropertyDescriptor(addressPD, homeAddr_child["CityName"], "HomeAddr_CityName");
            array[cols.Count + 1] = new SubPropertyDescriptor(addressPD, homeAddr_child["PostCode"], "HomeAddr_PostCode");

            PropertyDescriptorCollection newcols = new PropertyDescriptorCollection(array);

            return newcols;
        }

    }

    public class MyTypeDescriptionProvider : TypeDescriptionProvider
    {
        private ICustomTypeDescriptor td;

        public MyTypeDescriptionProvider()
            : this(TypeDescriptor.GetProvider(typeof(Message)))        {        }

        public MyTypeDescriptionProvider(TypeDescriptionProvider parent)
            : base(parent){}

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if (td == null)
            {
                td = base.GetTypeDescriptor(objectType, instance);
                td = new MyCustomTypeDescriptor(td);
            }

            return td;
        }
    }
}
