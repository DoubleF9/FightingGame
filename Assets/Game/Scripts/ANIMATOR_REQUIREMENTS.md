# Animation Requirements for Super Move

## For Animator: Super Attack Animation

The code expects an animation state called **`SuperAttackAnimation`** in each character's Animator Controller.

---

## Quick Temporary Solution

If you want to test immediately without creating new animations:

1. Open any character's Animator Controller (e.g., `DeadpoolAnimator.controller`)
2. Find the `Attack4Animation` state
3. Duplicate it (Ctrl+D)
4. Rename the duplicate to `SuperAttackAnimation`

This will use the existing heavy attack as the super move placeholder.

---

## Recommended: Using FightingMotionsVolume1 Animations

### Best Animations for Super Move

Located in: `Assets/FightingMotionsVolume1/FBX/`

| Animation File | Description | Why It's Good for Super |
|----------------|-------------|------------------------|
| **`hp_upper_right_A.fbx`** | Right uppercut | Classic fighting game super, launches opponent |
| **`hk_rh_right_A.fbx`** | High roundhouse kick | Powerful spinning kick |
| **`bk_push_left_A.fbx`** | Push/teep kick | Sends opponent flying back |
| **`hk_side_left_A.fbx`** | Side kick | Strong thrust kick |

### How to Add to Character

1. **Retarget the Animation:**
   
   a. In Project window, navigate to `Assets/FightingMotionsVolume1/FBX/`
   
   b. Select `hp_upper_right_A.fbx` (or your chosen animation)
   
   c. In Inspector, go to **Rig** tab
   
   d. Set **Animation Type** to **Humanoid**
   
   e. Click **Apply**
   
   f. Go to **Animation** tab
   
   g. Make sure **Loop Time** is **OFF** (super moves shouldn't loop)
   
   h. Click **Apply**

2. **Add to Animator Controller:**
   
   a. Open character's Animator Controller (e.g., `Assets/Game/Characters/Deadpool/DeadpoolAnimator.controller`)
   
   b. Double-click to open in Animator window
   
   c. Right-click in empty space → **Create State** → **Empty**
   
   d. Name it exactly: `SuperAttackAnimation`
   
   e. With the new state selected, look at Inspector:
      - Find **Motion** field
      - Drag the `hp_upper_right_A` animation clip into it
   
   f. The animation is imported from the FBX, look for it under the FBX file in Project window (expand the FBX with the arrow)

3. **Set Transition Back to Idle:**
   
   a. Right-click on `SuperAttackAnimation` state
   
   b. Select **Make Transition**
   
   c. Click on `IdleAnimation` state
   
   d. Select the transition arrow
   
   e. In Inspector:
      - **Has Exit Time**: ✓ (checked)
      - **Exit Time**: 0.9 (90% through animation)
      - **Transition Duration**: 0.1
      - **Conditions**: (leave empty - exits by time)

---

## Repeat for All Characters

You need to add `SuperAttackAnimation` to each character's Animator Controller:

- `Assets/Game/Characters/Bear/BearAnimator.controller`
- `Assets/Game/Characters/Deadpool/DeadpoolAnimator.controller`
- `Assets/Game/Characters/JackSkellington/JackAnimator.controller`
- `Assets/Game/Characters/JohnCena/JohnCenaAnimator.controller`
- `Assets/Game/Characters/Mario/MarioAnimator.controller`
- `Assets/Game/Characters/Patrick/PatrickAnimator.controller`
- `Assets/Game/Characters/Po/PoAnimator.controller`
- `Assets/Game/Characters/Rick/RickAnimator.controller`
- `Assets/Game/Characters/Shrek/ShrekAnimator.controller`

---

## Character-Specific Supers (Future Enhancement)

If you want unique supers per character later, create different animations:

| Character | Suggested Super Animation | Concept |
|-----------|--------------------------|---------|
| Deadpool | Something flashy with weapons | Breaking 4th wall |
| Shrek | Heavy slam | Ground pound |
| Mario | Flying punch | Jump attack |
| John Cena | Grapple/slam | Wrestling move |
| Patrick | Spin | Tornado spin |
| Po | Single powerful strike | Wuxi Finger Hold |
| Jack Skellington | Ghost animation | Spooky attack |
| Bear | Bear hug/maul | Grab attack |
| Rick | Scientific weapon | Portal/ray gun |

For now, the universal `hp_upper_right_A.fbx` uppercut works for all!

---

## Visual Reference: Expected Flow

```
[Player presses Q with full meter]
           ↓
[SuperAttackAnimation plays]     ← This is what you're creating
           ↓
[Hit detection occurs at ~30% of animation]
           ↓
[Transition back to IdleAnimation]
```

---

## Sound Effects to Use

Location: `Assets/Deadly Kombat Free version/`

| Sound | File | When It Plays |
|-------|------|---------------|
| Activation | `fire_punch_02.wav` | When super starts |
| Impact | `body_hit_finisher_42.wav` | When super hits |
| Extra | `bone_breaking_03.wav` | Optional extra impact |

These are assigned in the Unity Inspector, not in the Animator.

---

## Questions?

If you have any questions about implementing these animations, check the main guide:
`Assets/Game/Scripts/SUPER_MOVE_SETUP_GUIDE.md`
