using Vintagestory.API.Common;

[assembly: ModInfo("ImmersiveCrafting",
  Authors = new[] { "Craluminum2413" })]

namespace MoreShelves
{
  class MoreShelves : ModSystem
  {
    public override void Start(ICoreAPI api)
    {
      base.Start(api);
      api.RegisterBlockClass("BlockShelfOne", typeof(BlockShelfOne));
      api.RegisterBlockEntityClass("ShelfOne", typeof(BlockEntityShelfOne));
      // api.RegisterBlockClass("shelfone", typeof(ShelfTwo));
      // api.RegisterBlockEntityClass("shelfone", typeof(BEShelfTwo));
    }
  }
}