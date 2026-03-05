using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUITestProject.Extensions
{
    [MarkupExtensionReturnType(ReturnType =typeof(SymbolIconSource))]
    public sealed class SymbolIconSourceExtension:MarkupExtension
    {
        public Symbol Symbol { get; set; } = Symbol.Document;
        protected override object ProvideValue() => new SymbolIconSource();
    }
}
