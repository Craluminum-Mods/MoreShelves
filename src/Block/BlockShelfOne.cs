using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace MoreShelves
{
  public class BlockShelfOne : BlockShelf
  {
    public override void OnLoaded(ICoreAPI api)
    {
      base.OnLoaded(api);
    }

    public override bool DoParticalSelection(IWorldAccessor world, BlockPos pos)
    {
      return true;
    }

    public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
    {
      BlockEntityShelfOne beshelfone = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityShelfOne;
      if (beshelfone != null) return beshelfone.OnInteract(byPlayer, blockSel);

      return base.OnBlockInteractStart(world, byPlayer, blockSel);
    }
  }
}