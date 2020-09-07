using System;
using System.Collections.Generic;
using AnimLib.Animations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AnimLib {
  /// <summary>
  /// Main <see cref="ModPlayer"/> class for <see cref="AnimLibMod"/>, contains and updates <see cref="AnimationController"/>.
  /// </summary>
  public sealed class AnimPlayer : ModPlayer {
    internal Dictionary<Mod, AnimationController> animationControllers { get; private set; }

    /// <summary>
    /// Constructs and collects all <see cref="AnimationController"/>s across all mods onto this <see cref="Player"/>.
    /// </summary>
    public override void Initialize() {
      if (!AnimLoader.UseAnimations) {
        return;
      }

      animationControllers = new Dictionary<Mod, AnimationController>();
      AnimLoader.CreateAnimationControllersForPlayer(this);
    }

    /// <summary>
    /// Updates all <see cref="AnimationController"/>s on this <see cref="Player"/>.
    /// </summary>
    public override void PostUpdate() {
      if (!AnimLoader.UseAnimations) {
        return;
      }

      foreach (var anim in animationControllers.Values) {
        // Probably not a good idea to crash when a purely cosmetic effect fails.
        try {
          if (anim.PreUpdate()) {
            anim.Update();
          }
        }
        catch (Exception ex) {
          AnimLibMod.Instance.Logger.Error($"- :[{anim.mod.Name}]: Caught exception while updating animations for {anim.GetType().SafeTypeName(nameof(AnimationController))}.", ex);
          Main.NewText($"AnimLib -> {anim.mod.Name}: Caught exception while updating animations. See client.log for more information.", Color.Red);
        }
      }
    }
  }
}