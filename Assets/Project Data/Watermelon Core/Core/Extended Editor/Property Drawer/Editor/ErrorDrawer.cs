using UnityEditor;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(ErrorAttribute))]
    public class ErrorDrawer : HelpBoxDrawer
    {
        protected override MessageType GetMessageType()
        {
            return MessageType.Error;
        }
    }
}
