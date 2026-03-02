# Autonomous Walker

An AI-powered autonomous walking agent trained using reinforcement learning with Unity ML-Agents and PPO (Proximal Policy Optimization).

## Overview

This project demonstrates a machine learning agent learning to walk autonomously in a simulated 3D environment. The agent is trained using the Unity ML-Agents Toolkit with PPO reinforcement learning algorithm and can navigate obstacles and collect rewards.

## The Project
<video controls src="Trained Agent.mp4" title="Title"></video>

## Features

- **Autonomous Agent**: Trained using deep reinforcement learning (PPO)
- **3D Simulation**: Built in Unity with realistic physics
- **Obstacle Avoidance**: Agent learns to navigate around walls
- **Reward System**: Collects rewards ("apples") while walking
- **Exported Model**: Pre-trained ONNX model included for inference

## Prerequisites

### For Training
- **Python 3.8+**
- **Jupyter Notebook**
- **PyTorch**
- **ML-Agents Python Package**: `pip install mlagents`
- **Unity ML-Agents Toolkit**

### For Running in Unity
- **Unity 6.3 LTS** or compatible version
- **ML-Agents Unity Package**
- **Input System Package**
- **Universal Render Pipeline (URP)**

## Installation

### 1. Clone or Download the Repository
```bash
cd Autonomus-Walker
```

### 2. Set Up Python Environment (for Training)
```bash
python -m venv venv
venv\Scripts\activate
pip install -r requirements.txt
pip install mlagents torch jupyter
```

### 3. Open Unity Project
1. Install Unity Hub
2. Open the `Autonomus-Walker/Unity Files` folder as a Unity project
3. Import ML-Agents and required packages if not already imported

## Training

### Run Training in Jupyter
1. Navigate to `Training Files/` directory
2. Open `train.ipynb` in Jupyter Notebook:
   ```bash
   jupyter notebook train.ipynb
   ```
3. Follow the notebook cells to:
   - Load the training configuration from `config/AgentBehaviour.yaml`
   - Train the agent using PPO
   - Monitor training progress with TensorBoard

### Training Configuration
Edit `Training Files/config/AgentBehaviour.yaml` to adjust:
- Learning rate
- Network architecture
- Training steps
- Reward scaling
- Other PPO hyperparameters

## Running in Unity

1. **Open the Scene**: Open `Assets/Scenes/SampleScene.unity` in Unity Editor
2. **Configure Agent**: The `AgentBehaviour` script is already attached to the agent
3. **Load Model**: The pre-trained `AgentBehaviour.onnx` model is in `Assets/Scripts/`
4. **Play**: Press Play in the Unity Editor to see the trained agent in action

## Key Components

### AgentBehaviour.cs
Main script that:
- Defines observation space (what the agent sees)
- Defines action space (what the agent can do)
- Implements reward function
- Handles agent initialization and reset logic

### Apple.cs
Script for reward objects that:
- Trigger positive rewards when touched by agent
- Respawn randomly in the environment

### Wall.cs
Script for obstacle objects that:
- Block the agent's path
- Provide negative rewards for collision

## Training Results

- **Algorithm**: Proximal Policy Optimization (PPO)
- **Model**: Pre-trained ONNX format available in `Training Files/results/ppo/AgentBehaviour.onnx`
- **Status**: Model ready for inference in Unity

## How the Agent Works

1. **Observations**: The agent observes:
   - Its own position and velocity
   - Distance to obstacles (walls)
   - Direction and distance to reward objects (apples)

2. **Actions**: The agent can:
   - Adjust walking speed
   - Change direction
   - Jump or move its limbs (depending on implementation)

3. **Rewards**: 
   - Positive reward for collecting apples
   - Negative reward for hitting walls
   - Has movement efficiency bonuses

## Resources

- [Unity ML-Agents Documentation](https://docs.unity3d.com/Manual/com.unity.ml-agents.html)
- [ML-Agents GitHub Repository](https://github.com/Unity-Technologies/ml-agents)
- [PPO Algorithm Paper](https://arxiv.org/abs/1707.06347)

## License

See [LICENSE](LICENSE) file for details.

## Contributing

Feel free to submit issues or pull requests to improve the project.

## Author

Hari Prasath M