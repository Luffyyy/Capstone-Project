public class Helpers
{
    /*
        Handles any value in c# and converst it to a boolean value
    */
    public static bool VPLIsTrue(object value)
    {
        if (value == null) return false;

        return value switch
        {
            bool b => b,
            int i => i != 0,
            float f => f != 0.0f,
            string s => !string.IsNullOrEmpty(s),
            System.Collections.ICollection c => c.Count > 0,
            _ => true // Default for objects is true
        };
    }
}