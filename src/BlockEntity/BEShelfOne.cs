using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace MoreShelves
{
  public class BlockEntityShelfOne : BlockEntityShelf
  {
    private InventoryGeneric inv;
    Matrixf mat = new Matrixf();
    public override string AttributeTransformCode => "onShelfOneTransform";

    public BlockEntityShelfOne()
    {
      inv = new InventoryGeneric(1, "shelf-0", null, null);
      meshes = new MeshData[1];
    }

    internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
    {
      ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;

      if (slot.Empty)
      {
        if (TryTake(byPlayer, blockSel))
        {
          return true;
        }
        return false;
      }
      else
      {
        CollectibleObject colObj = slot.Itemstack.Collectible;
        if (colObj.Attributes != null && ((colObj.Attributes["shelvable"].AsBool(false) == true) || (colObj.Attributes["shelvableone"].AsBool(false) == true)))
        {
          AssetLocation sound = slot.Itemstack?.Block?.Sounds?.Place;

          if (TryPut(slot, blockSel))
          {
            Api.World.PlaySoundAt(sound != null ? sound : new AssetLocation("sounds/player/build"), byPlayer.Entity, byPlayer, true, 16);
            updateMeshes();
            return true;
          }

          return false;
        }
      }
      return false;
    }

    private bool TryPut(ItemSlot slot, BlockSelection blockSel)
    {
      if (inv[0].Empty)
      {
        int moved = slot.TryPutInto(Api.World, inv[0]);
        updateMeshes();
        MarkDirty(true);
        return moved > 0;
      }
      return false;
    }

    private bool TryTake(IPlayer byPlayer, BlockSelection blockSel)
    {

      if (!inv[0].Empty)
      {
        ItemStack stack = inv[0].TakeOut(1);
        if (byPlayer.InventoryManager.TryGiveItemstack(stack))
        {
          AssetLocation sound = stack.Block?.Sounds?.Place;
          Api.World.PlaySoundAt(sound != null ? sound : new AssetLocation("sounds/player/build"), byPlayer.Entity, byPlayer, true, 16);
        }

        if (stack.StackSize > 0)
        {
          Api.World.SpawnItemEntity(stack, Pos.ToVec3d().Add(0.5, 0.5, 0.5));
        }
        MarkDirty(true);
        updateMeshes();
        return true;
      }
      return false;
    }

    public override void TranslateMesh(MeshData mesh, int index)
    {
      float x = 4 / 16f;
      float y = 2 / 16f;
      float z = 4 / 16f;

      Vec4f offset = mat.TransformVector(new Vec4f(x - 0.5f, y, z - 0.5f, 0));
      mesh.Translate(offset.XYZ);
    }

  }
}