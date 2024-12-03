namespace Watermelon
{
    public sealed class DefaultSaveWrapper : BaseSaveWrapper
    {
        private const Serializer.SerializeType SAVE_SERIALIZE_TYPE = Serializer.SerializeType.Binary;

        public override GlobalSave Load(string fileName)
        {
            return Serializer.DeserializeFromPDP<GlobalSave>(fileName, SAVE_SERIALIZE_TYPE, logIfFileNotExists: false);
        }

        public override void Save(GlobalSave globalSave, string fileName)
        {
            Serializer.SerializeToPDP(globalSave, fileName, SAVE_SERIALIZE_TYPE);
        }

        public override void Delete(string fileName)
        {
            Serializer.DeleteFileAtPDP(fileName);
        }

        public override bool UseThreads()
        {
            return true;
        }
    }
}
