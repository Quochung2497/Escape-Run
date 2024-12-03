using UnityEditor;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(InfoAttribute))]
    public class InfoDrawer : HelpBoxDrawer
    {
        protected override MessageType GetMessageType()
        {
            return MessageType.Info;
        }
    }
}
