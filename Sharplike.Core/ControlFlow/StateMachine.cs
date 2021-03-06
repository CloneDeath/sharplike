﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Sharplike.Core.Input;

namespace Sharplike.Core.ControlFlow
{
	/// <summary>
	/// State machine with a collection of state stacks.
	/// </summary>
	public sealed class StateMachine
	{
		private String currentStack;
		private Dictionary<String, Stack<AbstractState>> stackDictionary;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="gameInstance">The instance for this game.</param>
		/// <param name="rootState">The root state to start the StateMachine on.</param>
		public StateMachine(AbstractState rootState)
		{
			currentStack = "main";
			stackDictionary = new Dictionary<String,Stack<AbstractState>>();
			this.CreateStack(currentStack, rootState);

			Game.GameProcessing += new EventHandler<EventArgs>(GameProcessing);

			Game.InputSystem.Command.CommandStarted += new EventHandler<CommandEventArgs>(InputSystem_CommandStarted);
			Game.InputSystem.Command.CommandEnded += new EventHandler<CommandEventArgs>(InputSystem_CommandEnded);
			Game.InputSystem.Command.CommandTriggered += new EventHandler<CommandEventArgs>(InputSystem_CommandTriggered);
			Game.InputSystem.InputProvider.OnKeyPressed += new EventHandler<KeyEventArgs>(InputProvider_OnKeyPressed);
		}
		
		/// <summary>
		/// The current state active in the StateMachine.
		/// </summary>
		public AbstractState CurrentState
		{
			get
			{
				return this.stackDictionary[currentStack].Peek();
			}
		}

		/// <summary>
		/// The string key of the currently active stack. The "main" stack is the
		/// primary stack in the application, but more may be added.
		/// </summary>
		public String CurrentStack
		{
			get
			{
				return currentStack;
			}
		}


		/// <summary>
		/// Creates a new stack within the StateMachine.
		/// </summary>
		/// <param name="stackName">The string key for the new stack.</param>
		/// <param name="rootState">The start state for the new stack.</param>
		public void CreateStack(String stackName, AbstractState rootState)
		{
			String s = stackName.ToLower();
			if (stackDictionary.ContainsKey(s))
				throw new ArgumentException("Stack '" + s + "' already exists in the state machine.");
			Stack<AbstractState> stack = new Stack<AbstractState>();
			stackDictionary.Add(s, stack);
			this.PushState(s, rootState);
		}

		/// <summary>
		/// Destroys any non-main, currently inactive stack.
		/// </summary>
		/// <param name="stackName">The string key of the stack to destroy.</param>
		public void DestroyStack(String stackName)
		{
			String s = stackName.ToLower();
			if (s == "main")
				throw new ArgumentException("You cannot invoke DestroyStack() on the main stack.");
			if (s == currentStack.ToLower())
				throw new ArgumentException("You cannot invoke DestroyStack() on the current stack. Switch stacks first.");
			if (stackDictionary.ContainsKey(s) == false)
				throw new ArgumentException("Stack '" + s + "' does not exist in the state machine.");

			stackDictionary.Remove(s);
		}

		/// <summary>
		/// Switches execution of the state machine to the specified stack.
		/// </summary>
		/// <param name="stackName">The string key of the stack to which the state machine shall switch execution.</param>
		public void SwitchStack(String stackName)
		{
			String s = stackName.ToString();
			if (stackDictionary.ContainsKey(s) == false)
				throw new ArgumentException("Stack '" + s + "' does not exist in the state machine.");

			this.stackDictionary[currentStack].Peek().StackLostFocus();

			currentStack = s;

			this.stackDictionary[currentStack].Peek().StackGotFocus();
		}

		/// <summary>
		/// Pushes a state onto the currently active stack (making it the new running state).
		/// </summary>
		/// <param name="newState">The state to add to the currently active stack.</param>
		public void PushState(AbstractState newState)
		{
			this.PushState(this.currentStack, newState);
		}

		/// <summary>
		/// Pushes a state onto the specified stack.
		/// </summary>
		/// <param name="stackName">The stack on which to place the state.</param>
		/// <param name="newState">The state to add to the specified stack.</param>
		public void PushState(String stackName, AbstractState newState)
		{
			String s = stackName.ToLower();
			if (stackDictionary.ContainsKey(s) == false)
				throw new ArgumentException("Stack '" + s + "' does not exist in the state machine.");

			if (s != "main" || this.stackDictionary["main"].Count > 0)
			{
				newState.Parent = this.stackDictionary[s].Peek();
				newState.Parent.StatePaused();
			}

			this.stackDictionary[s].Push(newState);
			newState.StateMachine = this;
			newState.StateStarted();
		}

		/// <summary>
		/// Pops a state off of the currently active stack. Control flow will
		/// return to the next most recent state in the stack, or will terminate
		/// execution of the game the last state in the main stack is popped.
		/// </summary>
		/// <returns>The state that has been popped off the stack.</returns>
		public AbstractState PopState()
		{
			return this.PopState(this.currentStack);
		}

		/// <summary>
		/// Pops a state off of any specified stack. Execution of the game will
		/// terminate if the last state is popped from the main stack, and an
		/// exception will be thrown if the last state in any other stack is
		/// popped.
		/// </summary>
		/// <param name="stackName">The stack from which to pop a state.</param>
		/// <returns>The state that has been popped off the stack.</returns>
		public AbstractState PopState(String stackName)
		{
			String s = stackName.ToLower();
			if (stackDictionary.ContainsKey(s) == false)
				throw new ArgumentException("Stack '" + s + "' does not exist in the state machine.");

			Stack<AbstractState> stack = this.stackDictionary[s];

			if (stack.Count <= 1 && s != "main")
				throw new InvalidOperationException("Non-main stacks may not pop their last method. Switch " +
													"to another stack and invoke DestroyStack() on the stack.");

			AbstractState oldState = this.stackDictionary[s].Pop();

			oldState.StateEnded();
			if (oldState.Parent != null)
			{
				oldState.Parent.StateResumed(oldState);
				//game.InputSystem.State = oldState.InputCommandSet;
			}

			if (this.stackDictionary["main"].Count == 0)
				Game.Terminate();

			return oldState;
		}

		

		private void GameProcessing(object sender, EventArgs e)
		{
			this.stackDictionary[currentStack].Peek().GameProcessing();
		}

		/// <summary>
		/// Calls the GameLoopTick on the currently active state.
		/// </summary>
		/// <param name="loop"></param>
		public void GameLoopTick(Sharplike.Core.Runtime.AbstractGameLoop loop)
		{
			this.stackDictionary[currentStack].Peek().GameLoopTick(loop);
		}


		void InputSystem_CommandTriggered(object sender, CommandEventArgs e)
		{
			this.stackDictionary[currentStack].Peek().CommandTriggered(e);
		}

		void InputSystem_CommandStarted(object sender, CommandEventArgs e)
		{
			this.stackDictionary[currentStack].Peek().CommandStarted(e);
		}

		void InputSystem_CommandEnded(object sender, CommandEventArgs e)
		{
			this.stackDictionary[currentStack].Peek().CommandEnded(e);
		}

		void InputProvider_OnKeyPressed(object sender, KeyEventArgs e)
		{
			this.stackDictionary[currentStack].Peek().KeyPressed(e);
		}
	}
}
