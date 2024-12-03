namespace Watermelon
{
    public abstract class BaseSaveWrapper
    {
        public static BaseSaveWrapper ActiveWrapper =
#if UNITY_EDITOR
            new DefaultSaveWrapper();
#elif UNITY_WEBGL
            new WebGLSaveWrapper();
#else
            new DefaultSaveWrapper();
#endif

        public abstract GlobalSave Load(string fileName);
        public abstract void Save(GlobalSave globalSave, string fileName);
        public abstract void Delete(string fileName);

        public virtual bool UseThreads() { return false; }
    }
}
