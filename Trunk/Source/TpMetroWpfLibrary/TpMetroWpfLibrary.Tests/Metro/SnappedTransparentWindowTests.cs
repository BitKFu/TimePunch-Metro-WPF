using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimePunch.Metro.Wpf.Metro;

namespace TpMetroWpfLibrary.Tests.Metro
{
    /// <summary>
    /// This testclass checks some special behaviour of the SnappedTransparentWindow
    /// </summary>
    [TestClass]
    public class SnappedTransparentWindowTests
    {
        /// <summary>
        /// Validates the Pin Style Setting
        /// </summary>
        [TestMethod]
        public void PinStyle_SetAlwaysOff_ValidateEventHandler()
        {
            // arrange
            var snapped = new SnappedTransparentWindow();
            
            // act
            snapped.PinStyle = PinStyle.AlwaysOff;
        }
    }
}
