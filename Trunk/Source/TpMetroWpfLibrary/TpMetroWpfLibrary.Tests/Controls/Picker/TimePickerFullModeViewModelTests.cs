using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.Metro.Wpf.Controls.Picker;
using TimePunch.MVVM.EventAggregation;
using TimePunch.MVVM.Events;
using TpMetroWpfLibrary.Tests.Controller;

namespace TpMetroWpfLibrary.Tests.Controls.Picker
{
    /// <summary>
    /// This testclass checks the TimePickerFullModeViewModel
    /// </summary>
    [TestClass]
    public class TimePickerFullModeViewModelTests : 
        IHandleMessage<TimePickerSelectRequest>,
        IHandleMessage<GoBackNavigationRequest>
    {
        /// <summary>
        /// Id of the event that will be fired to evaluate the check command
        /// </summary>
        private readonly Guid checkCommandId = new Guid("BE0BD065-5129-49C5-9BED-35D8FCBF8575");

        /// <summary>
        /// Check Command Event will be set, if the correct event gets catched
        /// </summary>
        private AutoResetEvent checkCommandEvent = new AutoResetEvent(false);

        /// <summary>
        /// Check Command Event will be set, if the correct event gets catched
        /// </summary>
        private AutoResetEvent cancelCommandEvent = new AutoResetEvent(false);

        /// <summary>
        /// Initializes the tests
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            Kernel.Instance = new StubKernel();
        }

        /// <summary>
        /// This test checks the contstructor and the correct initialization of the members
        /// </summary>
        [TestMethod]
        public void TimePickerFullModeViewModel_Constructor_Validate24Hours ()
        {
            // arrange & act
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = true };

            // assert
            Assert.AreEqual(2, vm.AllDesignators.Length, "We expected two designators AM/PM");
            Assert.AreEqual(24, vm.AllHours.Length, "We expected 24 hours");
            Assert.AreEqual(60, vm.AllMinutes.Length, "We expected 60 minutes");
        }

        /// <summary>
        /// This test checks the contstructor and the correct initialization of the members
        /// </summary>
        [TestMethod]
        public void TimePickerFullModeViewModel_Constructor_Validate12Hours ()
        {
            // arrange & act
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = true };

            // assert
            Assert.AreEqual(2, vm.AllDesignators.Length, "We expected two designators AM/PM");
            Assert.AreEqual(24, vm.AllHours.Length, "We expected 24 hours");
            Assert.AreEqual(60, vm.AllMinutes.Length, "We expected 60 minutes");
        }

        /// <summary>
        /// This method checks if the command binding is correct
        /// </summary>
        [TestMethod]
        public void Initialize_CommandBinding_ValidateBinding()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel();

            // act
            vm.Initialize();

            // assert
            Assert.IsNotNull(vm.CheckCommand, "The check command must be bound");
            Assert.IsNotNull(vm.CancelCommand, "The cancel command must be bound");
        }

        /// <summary>
        /// This method checks, if the InitializePage will return instantly if wrong data is send
        /// </summary>
        [TestMethod]
        public void InitializePage_UndefinedData_ReturnInstantly()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel();

            // act & assert (it's not allowed that any exception will be thrown)
            vm.InitializePage("Some invalid data");
        }

        /// <summary>
        /// This method checks, if the page has been correctly initialized depending on the given data
        /// </summary>
        [TestMethod]
        public void InitializePage_TimePickerFullModeRequest_DataHasBeenSet()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = false };
            var guid = new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753");
            var request = new TimePickerFullModeRequest("Header", new DateTime(2012, 10, 23, 15, 23, 22), guid);

            // act
            vm.InitializePage(request);

            // assert
            Assert.AreEqual("Header", request.FullModeHeader, "The header is not set as expected");
            Assert.AreEqual(vm.TimePickerId, guid, "The TimePicker id is not as requested");
            Assert.AreEqual(23, vm.SelectedMinute.Minute, "The minute has not been set as expected");
            Assert.AreEqual(15, vm.SelectedHour.Hour, "The hour has not been set as expected");
        }

        /// <summary>
        /// This method checks the round o´clock initialization
        /// </summary>
        [TestMethod]
        public void InitializePage_24Hours_CheckInitialization()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = false };
            var guid = new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753");
            var time = DateTime.Today; // start today at 0:00 

            while (time.Date == DateTime.Today)
            {
                // act
                var request = new TimePickerFullModeRequest("Header", time, guid);
                vm.InitializePage(request);

                // assert
                Assert.AreEqual(time.Hour, vm.SelectedHour.Hour, "The hour did not match");
                Assert.AreEqual(time.Minute, vm.SelectedMinute.Minute, "The minute did not match");

                // Add 1 Minute
                time = time.AddMinutes(1);
            }
        }

        /// <summary>
        /// This method checks the round o´clock initialization
        /// </summary>
        [TestMethod]
        public void InitializePage_24Hours_CheckUpdateTimeValue()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = false };
            var guid = new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753");
            var time = DateTime.Today; // start today at 0:00 

            while (time.Date == DateTime.Today)
            {
                // arrange
                var request = new TimePickerFullModeRequest("Header", time, guid);
                vm.InitializePage(request);
                
                // act
                vm.UpdateTimeValue();
                
                // assert
                Assert.AreEqual(time, vm.Value, "The time cound not set correctly");

                // Add 1 Minute
                time = time.AddMinutes(1);
            }
        }

        /// <summary>
        /// This method checks the round o´clock initialization
        /// </summary>
        [TestMethod]
        public void InitializePage_12Hours_CheckInitialization()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = true };
            var guid = new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753");
            var time = DateTime.Today; // start today at 0:00 

            while (time.Date == DateTime.Today)
            {
                // act
                var request = new TimePickerFullModeRequest("Header", time, guid);
                vm.InitializePage(request);

                // assert
                int awaitedHour = (time.Hour%12);
                if (awaitedHour == 0) awaitedHour = 12;

                Assert.AreEqual(awaitedHour, vm.SelectedHour.Hour, "The hour did not match");
                Assert.AreEqual(time.Minute, vm.SelectedMinute.Minute, "The minute did not match");

                // Add 1 Minute
                time = time.AddMinutes(1);
            }
        }

        /// <summary>
        /// This method checks the round o´clock initialization
        /// </summary>
        [TestMethod]
        public void InitializePage_12Hours_CheckUpdateTimeValue()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = true };
            var guid = new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753");
            var time = DateTime.Today; // start today at 0:00 

            while (time.Date == DateTime.Today)
            {
                // arrange
                var request = new TimePickerFullModeRequest("Header", time, guid);
                vm.InitializePage(request);

                // act
                vm.UpdateTimeValue();

                // assert
                Assert.AreEqual(time, vm.Value, "The time cound not set correctly");

                // Add 1 Minute
                time = time.AddMinutes(1);
            }
        }

        /// <summary>
        /// This method checks, if the hours are changed correct, if the designator changes
        /// </summary>
        [TestMethod]
        public void IsDesignatorVisible_SetTrue_CheckHours()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = false };
            var request = new TimePickerFullModeRequest("Header", DateTime.Today.AddHours(15), new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753"));

            // act
            vm.IsDesignatorVisible = true;
            vm.InitializePage(request);

            // assert
            Assert.AreEqual(3, vm.SelectedHour.Hour, "15:00 o'clock means 3 pm");
            Assert.AreEqual(1, vm.SelectedDesignator.DesignatorNumber, "Second designator");
        }

        /// <summary>
        /// This method checks, if the hours are changed correct, if the designator changes
        /// </summary>
        [TestMethod]
        public void IsDesignatorVisible_SetFalse_CheckHours()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = true };
            var request = new TimePickerFullModeRequest("Header", DateTime.Today.AddHours(15), new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753"));

            // act
            vm.IsDesignatorVisible = false;
            vm.InitializePage(request);

            // assert
            Assert.AreEqual(15, vm.SelectedHour.Hour, "15:00 o'clock means 3 pm");
            Assert.AreEqual(1, vm.SelectedDesignator.DesignatorNumber, "Second designator");
        }

        /// <summary>
        /// This method checks, when the selected hour changes, the resulting value changes too
        /// </summary>
        [TestMethod]
        public void SelectedHour_Change_CheckResultingValue()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = true };
            var request = new TimePickerFullModeRequest("Header", DateTime.Today.AddHours(12), new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753"));
            vm.InitializePage(request);

            // act
            vm.SelectedHour = vm.AllHours[15];

            // assert
            Assert.AreEqual(15, vm.Value.Hour, "Hours are set manually to 15 o´clock. so we expect to see the same");
        }

        /// <summary>
        /// This method checks, when the selected hour changes, the resulting value changes too
        /// </summary>
        [TestMethod]
        public void SelectedMinute_Change_CheckResultingValue()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = true };
            var request = new TimePickerFullModeRequest("Header", DateTime.Today.AddMinutes(12), new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753"));
            vm.InitializePage(request);

            // act
            vm.SelectedMinute = vm.AllMinutes[46];

            // assert
            Assert.AreEqual(46, vm.Value.Minute, "Hours are set manually to 15 o´clock. so we expect to see the same");
        }

        /// <summary>
        /// This method checks, when the selected hour changes, the resulting value changes too
        /// </summary>
        [TestMethod]
        public void SelectedDesignator_Change_CheckResultingValue()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel { IsDesignatorVisible = true };
            var request = new TimePickerFullModeRequest("Header", DateTime.Today.AddHours(3), new Guid("B43E7314-988E-41A8-B4E4-A02BA93FC753"));
            vm.InitializePage(request);

            // act
            vm.SelectedDesignator = vm.AllDesignators[1];   

            // assert
            Assert.AreEqual(15, vm.Value.Hour, "The resulting value must switch from 3 to 15 o´clock");
        }

        /// <summary>
        /// Checks, if the check command executes, that the time picker select request has been fired
        /// </summary>
        [TestMethod]
        public void CheckCommand_Execute_TimePickerSelectRequestHasBeenFired()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel();
            vm.Initialize();
            vm.InitializePage(new TimePickerFullModeRequest("Header", DateTime.Today.AddHours(3), checkCommandId));

            Kernel.Instance.EventAggregator.Subscribe(this);
            try
            {
                // Execute command and send the event
                vm.CheckCommand.Execute(null);

                // Wait 100ms for event
                checkCommandEvent.WaitOne(100);
            }
            finally 
            {
                Kernel.Instance.EventAggregator.Unsubscribe(this);
            }
        }   
        
        /// <summary>
        /// Checks, if the cancel command executes, that the time picker sends a GoBackNavigationRequest
        /// </summary>
        [TestMethod]
        public void CancelCommand_Execute_GoBackNavigationRequestHasBeenFired()
        {
            // arrange
            var vm = new TimePickerFullModeViewModel();
            vm.Initialize();
            vm.InitializePage(new TimePickerFullModeRequest("Header", DateTime.Today.AddHours(3), checkCommandId));

            Kernel.Instance.EventAggregator.Subscribe(this);
            try
            {
                // Execute command and send the event
                vm.CancelCommand.Execute(null);

                // Wait 100ms for event
                cancelCommandEvent.WaitOne(100);
            }
            finally 
            {
                Kernel.Instance.EventAggregator.Unsubscribe(this);
            }
        }

        #region Implementation of IHandleMessage<TimePickerSelectRequest>

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public void Handle(TimePickerSelectRequest message)
        {
            // Set the proper event 
            if (message.TimePickerId == checkCommandId)
                checkCommandEvent.Set();
        }

        #endregion

        #region Implementation of IHandleMessage<GoBackNavigationRequest>

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public void Handle(GoBackNavigationRequest message)
        {
            cancelCommandEvent.Set();
        }

        #endregion
    }
}
