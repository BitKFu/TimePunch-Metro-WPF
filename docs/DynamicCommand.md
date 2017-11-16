# Dynamic Commands
Dynamic Commands are commands that can be used like Relay Commands, but with an Execute Action and optional CanExecute Function, in opposite of the methods used by the Relay Commands. This advantage can be used for the ErrorControl, because the last executed method can be bound dynamically to the ErrorControl.

**Example for creating dynamic commands within an exception handler**
This examples shows how to create a dynamic command that can be bound to an Error Control.
{{
public void DoSomething()
{
   try
   {
      // Try to load some data from the internet
   }
   catch (Exception exc)
   {
      Error = exc.message;
      FaultedCommand = new DynamicCommand( ()=>DoSomething() );
   }
}
}}