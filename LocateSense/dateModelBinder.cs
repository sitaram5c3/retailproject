using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace LocateSense
{


 public class MyBinder 
 {

     public class DateTimeModelBinder : DefaultModelBinder
     {
         private string _customFormat;

         public DateTimeModelBinder(string customFormat)
         {
             _customFormat = customFormat;
         }

         public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
         {
             //var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
             var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
             var test = value.AttemptedValue; //provides the raw value
            // return value.AttemptedValue;
           //  return DateTime.ParseExact(value.AttemptedValue, _customFormat, CultureInfo.InvariantCulture);
             return DateTime.Now;
         }
     }


 }

}
