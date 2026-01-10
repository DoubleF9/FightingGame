# Super Move System - Setup Guide

This guide explains how to set up the Super Move system in your Unity project.

## 📁 Files Created

The following scripts were added:

| File | Location | Purpose |
|------|----------|---------|
| `SuperMeter.cs` | `Assets/Game/Scripts/FightingController/` | Handles player super meter logic |
| `SuperMeterAI.cs` | `Assets/Game/Scripts/Opponent/` | Handles AI opponent super meter logic |
| `SuperMeterUI.cs` | `Assets/Game/Scripts/UI/` | Displays the super meter bar in UI |
| `ScreenEffects.cs` | `Assets/Game/Scripts/FightingController/` | Camera shake, slow motion, hit stop effects |

The following scripts were modified:
- `FightingController.cs` - Added SuperMeter integration
- `OpponentAI.cs` - Added SuperMeterAI integration

---

## 🎮 How It Works

### Super Meter Charging
The super meter fills up through these actions:
- **Hitting opponent**: +10% meter
- **Taking damage**: +5% meter (rage mechanic)
- **Dodging**: +3% meter
- **Passive**: +1% per second

### Super Move Activation
- **Player**: Press `Q` when the super bar is full (100%)
- **AI**: Automatically uses super when ready and player is in range

### Super Move Effects
1. Brief hit-stop (freeze frame)
2. Slow motion during the attack
3. Screen shake on impact
4. 40 damage (vs normal 5 damage attacks!)
5. Knockback pushes opponent away

---

## 🔧 SETUP INSTRUCTIONS

### Step 1: Add ScreenEffects to Your Scene

The ScreenEffects script needs to exist in your scene for camera shake and slow motion.

1. In Unity, open your battle scene (e.g., `Map1`, `Map2`, or `Map3`)
2. Create an empty GameObject: `Right-click in Hierarchy → Create Empty`
3. Name it `ScreenEffects`
4. Add the ScreenEffects component: `Add Component → Scripts → ScreenEffects`
5. Drag your `Main Camera` into the `Main Camera` field in the Inspector

### Step 2: Add SuperMeter to Player Characters

For each player character (the ones controlled by FightingController):

1. Select the player character in the Hierarchy
2. Add the SuperMeter component: `Add Component → Scripts → SuperMeter`
3. Configure these fields in the Inspector:

| Field | Value | Notes |
|-------|-------|-------|
| Max Meter | 100 | Default is fine |
| Meter Gain On Hit | 10 | Charge when hitting |
| Meter Gain On Damage | 5 | Charge when hit (rage) |
| Meter Gain On Dodge | 3 | Reward for dodging |
| Passive Meter Gain | 1 | Per second |
| Super Damage | 40 | Damage dealt by super |
| Super Knockback | 5 | How far opponent flies back |
| Super Radius | 3 | Range of super attack |
| Super Animation Name | `Attack4Animation` | Use existing attack for quick test |
| Super Activation Key | Q | Key to press for super |
| Super Activation Sound | (see Step 5) | Optional |
| Super Impact Sound | (see Step 5) | Optional |

**Note:** You do NOT need to set the Opponents array! The script automatically uses the opponents from FightingController.

4. Link the SuperMeter to FightingController:
   - Select the player character
   - In the `FightingController` component, find the `Super Meter` field
   - Drag the same GameObject (or the SuperMeter component) into this field

### Step 3: Add SuperMeterAI to AI Opponents

For each AI opponent (the ones with OpponentAI):

1. Select the opponent character in the Hierarchy
2. Add the SuperMeterAI component: `Add Component → Scripts → SuperMeterAI`
3. Configure fields similar to SuperMeter (see Step 2)

**Note:** You do NOT need to set the Players array! The script automatically uses the players from OpponentAI.

4. Link to OpponentAI:
   - In the `OpponentAI` component, find the `Super Meter AI` field
   - Drag the SuperMeterAI component into this field

### Step 4: Create the Super Bar UI

1. In your Canvas (the one with health bars), create a new UI element:
   - `Right-click on Canvas → UI → Slider`
   - Name it `SuperMeterSlider_P1` (for player 1)

2. Position it below the health bar:
   - Anchor it to top-left (for player 1) or top-right (for AI)
   - Make it smaller than the health bar (e.g., half the width)

3. Customize the slider appearance:
   - Select `SuperMeterSlider_P1 → Background`
   - Change the color to dark gray
   - Select `SuperMeterSlider_P1 → Fill Area → Fill`
   - Change the color to blue (this will pulse gold when ready)

4. Add the SuperMeterUI script:
   - Select `SuperMeterSlider_P1`
   - `Add Component → Scripts → SuperMeterUI`
   - Drag the Slider component into `Super Meter Slider`
   - Drag the Fill image into `Fill Image`

5. (Optional) Create a "READY!" text:
   - Add a Text child to the slider: `Right-click on SuperMeterSlider_P1 → UI → Text`
   - Type "READY!" or "[Q]"
   - Drag this into the `Ready Indicator` field
   - It will automatically appear when super is full

6. Link UI to SuperMeter:
   - Select your player character
   - In the `SuperMeter` component, drag your `SuperMeterSlider_P1` into the `Super Meter UI` field

7. Repeat for AI opponent (SuperMeterSlider_AI)

### Step 5: Set Up Animations

#### IMPORTANT: Animation Names

The super move system expects an animation called `SuperAttackAnimation` in each character's Animator.

**Option A: Use Existing Attack Animation (Quick Test)**

To quickly test, you can change the `Super Animation Name` field to use an existing animation:
- Set it to `Attack4Animation` (uses your existing heavy attack)

**Option B: Add New Super Animation from FightingMotionsVolume1**

Location of recommended animations for super moves:
```
Assets/FightingMotionsVolume1/FBX/hp_upper_right_A.fbx    ← RECOMMENDED (Uppercut)
Assets/FightingMotionsVolume1/FBX/hk_rh_right_A.fbx       ← Alternative (Roundhouse kick)
Assets/FightingMotionsVolume1/FBX/bk_push_left_A.fbx      ← Alternative (Push kick)
```

**For your animator to add the super animation:**

1. Open a character's Animator Controller (e.g., `DeadpoolAnimator.controller`)
2. Right-click in the Animator window → `Create State → Empty`
3. Name the new state `SuperAttackAnimation`
4. In the Inspector for this state:
   - Set `Motion` to the FBX animation (e.g., `hp_upper_right_A`)
   - The animation from FightingMotionsVolume1 needs to be retargeted first

**Animation Retargeting (FOR ANIMATOR):**

The FightingMotionsVolume1 animations are made for "BlueGuy" rig. To use them:

1. Select an FBX file (e.g., `hp_upper_right_A.fbx`)
2. In Inspector → Rig tab:
   - Set `Animation Type` to `Humanoid`
   - Click `Apply`
3. Go to Animation tab:
   - Check `Loop Time` OFF for attacks
   - Click `Apply`
4. Now the animation can be used in any Humanoid character's Animator

### Step 6: Add Sound Effects

Location of recommended sound effects:
```
Assets/Deadly Kombat Free version/fire_punch_02.wav           ← Super activation sound
Assets/Deadly Kombat Free version/body_hit_finisher_42.wav    ← Super impact sound
Assets/Deadly Kombat Free version/bone_breaking_03.wav        ← Extra impact sound
```

1. Select your player character
2. In the `SuperMeter` component:
   - Drag `fire_punch_02.wav` into `Super Activation Sound`
   - Drag `body_hit_finisher_42.wav` into `Super Impact Sound`

---

## ✅ Verification Checklist

After setup, verify everything is connected:

### In Scene:
- [ ] `ScreenEffects` GameObject exists with ScreenEffects script
- [ ] ScreenEffects has Main Camera assigned

### For Each Player Character:
- [ ] Has `SuperMeter` component
- [ ] SuperMeter has `Super Meter UI` assigned
- [ ] SuperMeter has `Opponents` array filled
- [ ] FightingController has `Super Meter` field assigned
- [ ] Optional: Super sounds assigned

### For Each AI Opponent:
- [ ] Has `SuperMeterAI` component
- [ ] SuperMeterAI has `Super Meter UI` assigned
- [ ] SuperMeterAI has `Players` and `FightingController[]` filled
- [ ] OpponentAI has `Super Meter AI` field assigned
- [ ] Optional: Super sounds assigned

### In Canvas:
- [ ] Super meter slider exists for player
- [ ] Super meter slider exists for AI (optional)
- [ ] Sliders have `SuperMeterUI` component
- [ ] SuperMeterUI has slider and fill image assigned

### In Animator Controllers:
- [ ] Each character Animator has `SuperAttackAnimation` state
  - (Or change the `Super Animation Name` field to an existing animation)

---

## 🎯 Testing

1. Press Play in Unity
2. Attack the opponent several times - watch the super meter fill
3. When the bar turns gold and pulses, press `Q`
4. You should see:
   - Brief freeze (hit stop)
   - Slow motion effect
   - Screen shake on impact
   - 40 damage dealt to opponent
   - Super meter empties

---

## 🐛 Troubleshooting

**Super meter not filling:**
- Check that `SuperMeter` is assigned in `FightingController`
- Check the Console for any error messages

**Super not activating when pressing Q:**
- Verify the meter is at 100% (bar should be pulsing gold)
- Check `Super Activation Key` is set to `Q`
- Check Console for "SUPER READY!" message

**No screen effects:**
- Verify `ScreenEffects` GameObject exists in scene
- Verify `Main Camera` is assigned in ScreenEffects

**No animation playing:**
- Check that `Super Animation Name` matches an animation state in the Animator
- For quick test, set it to `Attack4Animation`

**No sound:**
- Audio clips must be assigned in Inspector
- Check that audio source exists in scene

---

## 📝 Summary

**Controls:**
- `1, 2, 3, 4` - Normal attacks (charge super meter)
- `E` - Dodge (charges super meter)
- `Q` - **SUPER MOVE** (when bar is full)

**The meter fills when you:**
- Hit the opponent
- Take damage (rage mechanic)  
- Dodge attacks
- Wait (passive gain)

Enjoy your new Super Move system! 🎮
