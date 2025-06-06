# 🤖 Robot AR Assembly App

This is an interactive Augmented Reality (AR) application built in Unity where users can **assemble robot parts** in 3D space and trigger animations after successful assembly. The experience is designed for educational and interactive tech demonstrations.

---

## 📱 Features

- 🔧 Drag & Drop robot parts onto fixed points
- ✨ Snap-to-fit functionality using distance thresholds
- 💥 Explode and Reset view of all parts
- 🧠 Parts lock into place once correctly assembled
- 📌 Popup notification on successful full assembly
- 🎞️ Idle animation ("wave") plays infinitely after popup is dismissed

---

## 🚀 How It Works

1. On start, robot parts appear separated (exploded view).
2. Users drag each part toward its respective position on the robot body.
3. When all parts are placed correctly:
   - A popup panel appears.
4. When the user closes the popup:
   - The robot begins waving using a looping animation.

---

## 🧰 Tools & Technologies

- **Unity** (2021 or newer recommended)
- **C#**
- **AR Foundation** (if extended to ARCore/ARKit)
- **Animator** for looping animations
- **UI System** for popups and interactions

---

## 🎮 How to Use

1. **Open the Project in Unity**
2. Assign:
   - `DetachedParts`: The parts users will move
   - `FixedPoints`: The correct snap locations
   - `ExplodePlane`: Where parts spread out at the beginning
3. Attach the Animator with the "wave" animation and ensure:
   - It’s marked as **Loop Time**
   - It's named exactly `"wave"` and matches what’s used in script
4. Play the scene:
   - Drag parts
   - Assemble
   - Close popup
   - Watch the robot animate 🎉

---

