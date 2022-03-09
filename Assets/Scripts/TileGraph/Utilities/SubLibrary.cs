namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public abstract class SubLibrary
        {
            protected FunctionLibrary _FunctionLibrary;

            public SubLibrary(FunctionLibrary functionLibrary)
            {
                _FunctionLibrary = functionLibrary;
            }
        }
    }
}
