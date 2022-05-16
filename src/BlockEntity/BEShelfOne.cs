using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace MoreShelves
{
  public class BlockEntityShelfOne : BlockEntityShelf
  {
    private InventoryGeneric inv;
    public override InventoryBase Inventory => inv;

    public override string AttributeTransformCode => "onShelfOneTransform";

    Block block;
    Matrixf mat = new Matrixf();
    private int rotation;
    static readonly Vec3f centre = new Vec3f(0.5f, 0, 0.5f);

    public BlockEntityShelfOne()
    {
      inv = new InventoryGeneric(1, "shelf-0", null, null);
      meshes = new MeshData[1];
    }

    public override void Initialize(ICoreAPI api)
    {
      base.Initialize(api);

      block = api.World.BlockAccessor.GetBlock(Pos);
      mat.RotateYDeg(block.Shape.rotateY);
      this.SetRotation();
    }

    private void SetRotation()
    {
      switch (Block.Variant["side"])
      {
        case "south": this.rotation = 180; break;
        case "west": this.rotation = 90; break;
        case "east": this.rotation = 270; break;
        default: this.rotation = 0; break;
      }
    }

    public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
    {
      base.FromTreeAttributes(tree, worldForResolving);
      rotation = tree.GetInt("rota");
    }

    public override void ToTreeAttributes(ITreeAttribute tree)
    {
      base.ToTreeAttributes(tree);
      tree.SetInt("rota", rotation);
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
        if (colObj.Attributes != null && colObj.Attributes["shelvableone"].AsBool(false) == true)
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

      Vec4f offset = mat.TransformVector(new Vec4f(x - 0.25f, y, z - 0.25f, 0));
      mesh.Translate(offset.XYZ);
      if (this.rotation > 0) mesh.Rotate(centre, 0, rotation * GameMath.DEG2RAD, 0);
    }

  }
}