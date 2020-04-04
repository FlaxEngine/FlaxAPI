// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Particle system parameter.
    /// </summary>
    /// <seealso cref="PersistentScriptingObject" />
    [Tooltip("Particle system parameter.")]
    public unsafe partial class ParticleEffectParameter : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected ParticleEffectParameter() : base()
        {
        }

        /// <summary>
        /// Gets the index of the emitter (not the emitter track but the emitter).
        /// </summary>
        [Tooltip("The index of the emitter (not the emitter track but the emitter).")]
        public int EmitterIndex
        {
            get { return Internal_GetEmitterIndex(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetEmitterIndex(IntPtr obj);

        /// <summary>
        /// Gets the emitter that this parameter belongs to.
        /// </summary>
        [Tooltip("The emitter that this parameter belongs to.")]
        public ParticleEmitter Emitter
        {
            get { return Internal_GetEmitter(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEmitter Internal_GetEmitter(IntPtr obj);

        /// <summary>
        /// Gets the parameter index (in the emitter parameters list).
        /// </summary>
        [Tooltip("The parameter index (in the emitter parameters list).")]
        public int ParamIndex
        {
            get { return Internal_GetParamIndex(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetParamIndex(IntPtr obj);

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        [Tooltip("The parameter type.")]
        public GraphParamType ParamType
        {
            get { return Internal_GetParamType(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GraphParamType Internal_GetParamType(IntPtr obj);

        /// <summary>
        /// Gets the parameter unique ID.
        /// </summary>
        [Tooltip("The parameter unique ID.")]
        public Guid ParamIdentifier
        {
            get { Internal_GetParamIdentifier(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetParamIdentifier(IntPtr obj, out Guid resultAsRef);

        /// <summary>
        /// Gets the emitter track name.
        /// </summary>
        [Tooltip("The emitter track name.")]
        public string TrackName
        {
            get { return Internal_GetTrackName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetTrackName(IntPtr obj);

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        [Tooltip("The parameter name.")]
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);

        /// <summary>
        /// Gets the parameter flag that indicates whenever it's exposed to public.
        /// </summary>
        [Tooltip("The parameter flag that indicates whenever it's exposed to public.")]
        public bool IsPublic
        {
            get { return Internal_GetIsPublic(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsPublic(IntPtr obj);

        /// <summary>
        /// Gets the default value of the parameter (set in particle system asset).
        /// </summary>
        [Tooltip("The default value of the parameter (set in particle system asset).")]
        public object DefaultValue
        {
            get { return Internal_GetDefaultValue(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetDefaultValue(IntPtr obj);

        /// <summary>
        /// Gets the default value of the parameter (set in particle emitter asset).
        /// </summary>
        [Tooltip("The default value of the parameter (set in particle emitter asset).")]
        public object DefaultEmitterValue
        {
            get { return Internal_GetDefaultEmitterValue(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetDefaultEmitterValue(IntPtr obj);

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        [Tooltip("The value of the parameter.")]
        public object Value
        {
            get { return Internal_GetValue(unmanagedPtr); }
            set { Internal_SetValue(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetValue(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetValue(IntPtr obj, object value);
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The particle system instance that plays the particles simulation in the game.
    /// </summary>
    [Tooltip("The particle system instance that plays the particles simulation in the game.")]
    public unsafe partial class ParticleEffect : Actor
    {
        /// <inheritdoc />
        protected ParticleEffect() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="ParticleEffect"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static ParticleEffect New()
        {
            return Internal_Create(typeof(ParticleEffect)) as ParticleEffect;
        }

        /// <summary>
        /// The particle system to play.
        /// </summary>
        [EditorDisplay("Particle Effect"), DefaultValue(null), EditorOrder(0)]
        [Tooltip("The particle system to play.")]
        public ParticleSystem ParticleSystem
        {
            get { return Internal_GetParticleSystem(unmanagedPtr); }
            set { Internal_SetParticleSystem(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleSystem Internal_GetParticleSystem(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParticleSystem(IntPtr obj, IntPtr value);

        /// <summary>
        /// The custom render task used as a view information source (effect will use its render buffers and rendering resolution information for particles simulation).
        /// </summary>
        [NoSerialize, HideInEditor]
        [Tooltip("The custom render task used as a view information source (effect will use its render buffers and rendering resolution information for particles simulation).")]
        public SceneRenderTask CustomViewRenderTask
        {
            get { return Internal_GetCustomViewRenderTask(unmanagedPtr); }
            set { Internal_SetCustomViewRenderTask(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SceneRenderTask Internal_GetCustomViewRenderTask(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCustomViewRenderTask(IntPtr obj, IntPtr value);

        /// <summary>
        /// The particles simulation update mode. Defines how to update particles emitter.
        /// </summary>
        [EditorDisplay("Particle Effect"), DefaultValue(SimulationUpdateMode.Realtime), EditorOrder(10)]
        [Tooltip("The particles simulation update mode. Defines how to update particles emitter.")]
        public SimulationUpdateMode UpdateMode
        {
            get { return Internal_GetUpdateMode(unmanagedPtr); }
            set { Internal_SetUpdateMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SimulationUpdateMode Internal_GetUpdateMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUpdateMode(IntPtr obj, SimulationUpdateMode value);

        /// <summary>
        /// The fixed timestep for simulation updates. Used only if UpdateMode is set to FixedTimestep.
        /// </summary>
        [EditorDisplay("Particle Effect"), DefaultValue(1.0f / 60.0f), EditorOrder(20), VisibleIf(nameof(IsFixedTimestep))]
        [Tooltip("The fixed timestep for simulation updates. Used only if UpdateMode is set to FixedTimestep.")]
        public float FixedTimestep
        {
            get { return Internal_GetFixedTimestep(unmanagedPtr); }
            set { Internal_SetFixedTimestep(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFixedTimestep(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFixedTimestep(IntPtr obj, float value);

        /// <summary>
        /// The particles simulation speed factor. Scales the particle system update delta time. Can be used to speed up or slow down the particles.
        /// </summary>
        [EditorDisplay("Particle Effect"), DefaultValue(1.0f), EditorOrder(30)]
        [Tooltip("The particles simulation speed factor. Scales the particle system update delta time. Can be used to speed up or slow down the particles.")]
        public float SimulationSpeed
        {
            get { return Internal_GetSimulationSpeed(unmanagedPtr); }
            set { Internal_SetSimulationSpeed(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSimulationSpeed(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSimulationSpeed(IntPtr obj, float value);

        /// <summary>
        /// Determines whether the particle effect should take into account the global game time scale for simulation updates.
        /// </summary>
        [EditorDisplay("Particle Effect"), DefaultValue(true), EditorOrder(40)]
        [Tooltip("Determines whether the particle effect should take into account the global game time scale for simulation updates.")]
        public bool UseTimeScale
        {
            get { return Internal_GetUseTimeScale(unmanagedPtr); }
            set { Internal_SetUseTimeScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseTimeScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseTimeScale(IntPtr obj, bool value);

        /// <summary>
        /// Determines whether the particle effect should loop when it finishes playing.
        /// </summary>
        [EditorDisplay("Particle Effect"), DefaultValue(false), EditorOrder(50)]
        [Tooltip("Determines whether the particle effect should loop when it finishes playing.")]
        public bool IsLooping
        {
            get { return Internal_GetIsLooping(unmanagedPtr); }
            set { Internal_SetIsLooping(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsLooping(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsLooping(IntPtr obj, bool value);

        /// <summary>
        /// If true, the particle simulation will be updated even when an actor cannot be seen by any camera. Otherwise, the simulation will stop running when the actor is off-screen.
        /// </summary>
        [EditorDisplay("Particle Effect"), DefaultValue(true), EditorOrder(60)]
        [Tooltip("If true, the particle simulation will be updated even when an actor cannot be seen by any camera. Otherwise, the simulation will stop running when the actor is off-screen.")]
        public bool UpdateWhenOffscreen
        {
            get { return Internal_GetUpdateWhenOffscreen(unmanagedPtr); }
            set { Internal_SetUpdateWhenOffscreen(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUpdateWhenOffscreen(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUpdateWhenOffscreen(IntPtr obj, bool value);

        /// <summary>
        /// The draw passes to use for rendering this object.
        /// </summary>
        [EditorDisplay("Particle Effect"), EditorOrder(75), DefaultValue(DrawPass.Default)]
        [Tooltip("The draw passes to use for rendering this object.")]
        public DrawPass DrawModes
        {
            get { return Internal_GetDrawModes(unmanagedPtr); }
            set { Internal_SetDrawModes(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DrawPass Internal_GetDrawModes(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrawModes(IntPtr obj, DrawPass value);

        /// <summary>
        /// Gets the effect parameters collection. Those parameters are instanced from the <see cref="ParticleSystem"/> that contains a linear list of emitters and every emitter has a list of own parameters.
        /// </summary>
        [Tooltip("The effect parameters collection. Those parameters are instanced from the <see cref=\"ParticleSystem\"/> that contains a linear list of emitters and every emitter has a list of own parameters.")]
        public ParticleEffectParameter[] Parameters
        {
            get { return Internal_GetParameters(unmanagedPtr, typeof(ParticleEffectParameter)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffectParameter[] Internal_GetParameters(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the effect parameters collection version number. It can be used to track parameters changes that occur when particle system or one of the emitters gets reloaded/edited.
        /// </summary>
        [Tooltip("The effect parameters collection version number. It can be used to track parameters changes that occur when particle system or one of the emitters gets reloaded/edited.")]
        public uint ParametersVersion
        {
            get { return Internal_GetParametersVersion(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern uint Internal_GetParametersVersion(IntPtr obj);

        /// <summary>
        /// Gets or sets the current time position of the particle system timeline animation playback (in seconds).
        /// </summary>
        [NoSerialize, HideInEditor]
        [Tooltip("The current time position of the particle system timeline animation playback (in seconds).")]
        public float Time
        {
            get { return Internal_GetTime(unmanagedPtr); }
            set { Internal_SetTime(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetTime(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTime(IntPtr obj, float time);

        /// <summary>
        /// Gets or sets the last game time when particle system was updated. Value -1 indicates no previous updates.
        /// </summary>
        [NoSerialize, HideInEditor]
        [Tooltip("The last game time when particle system was updated. Value -1 indicates no previous updates.")]
        public float LastUpdateTime
        {
            get { return Internal_GetLastUpdateTime(unmanagedPtr); }
            set { Internal_SetLastUpdateTime(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetLastUpdateTime(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLastUpdateTime(IntPtr obj, float time);

        /// <summary>
        /// Gets the CPU particles count (total).
        /// </summary>
        [Tooltip("The CPU particles count (total).")]
        public int CPUParticlesCount
        {
            get { return Internal_GetCPUParticlesCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetCPUParticlesCount(IntPtr obj);

        /// <summary>
        /// Exposed parameters overrides for Editor Undo.
        /// </summary>
        [HideInEditor, Serialize]
        [Tooltip("Exposed parameters overrides for Editor Undo.")]
        protected ParticleEffect.ParameterOverride[] ParametersOverrides
        {
            get { return Internal_GetParametersOverrides(unmanagedPtr, typeof(ParticleEffect.ParameterOverride)); }
            set { Internal_SetParametersOverrides(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffect.ParameterOverride[] Internal_GetParametersOverrides(IntPtr obj, System.Type resultArrayItemType0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParametersOverrides(IntPtr obj, ParticleEffect.ParameterOverride[] value);

        /// <summary>
        /// Gets the particle parameter.
        /// </summary>
        /// <param name="emitterTrackName">The emitter track name (in particle system asset).</param>
        /// <param name="paramName">The emitter parameter name (in particle emitter asset).</param>
        /// <returns>The effect parameter or null if failed to find.</returns>
        public ParticleEffectParameter GetParameter(string emitterTrackName, string paramName)
        {
            return Internal_GetParameter(unmanagedPtr, emitterTrackName, paramName);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffectParameter Internal_GetParameter(IntPtr obj, string emitterTrackName, string paramName);

        /// <summary>
        /// Gets the particle parameter.
        /// </summary>
        /// <param name="emitterTrackName">The emitter track name (in particle system asset).</param>
        /// <param name="paramId">The emitter parameter ID (in particle emitter asset).</param>
        /// <returns>The effect parameter or null if failed to find.</returns>
        public ParticleEffectParameter GetParameter(string emitterTrackName, Guid paramId)
        {
            return Internal_GetParameter1(unmanagedPtr, emitterTrackName, ref paramId);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEffectParameter Internal_GetParameter1(IntPtr obj, string emitterTrackName, ref Guid paramId);

        /// <summary>
        /// Gets the particle parameter value.
        /// </summary>
        /// <param name="emitterTrackName">The emitter track name (in particle system asset).</param>
        /// <param name="paramName">The emitter parameter name (in particle emitter asset).</param>
        /// <returns>The value.</returns>
        public object GetParameterValue(string emitterTrackName, string paramName)
        {
            return Internal_GetParameterValue(unmanagedPtr, emitterTrackName, paramName);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetParameterValue(IntPtr obj, string emitterTrackName, string paramName);

        /// <summary>
        /// Set the particle parameter value.
        /// </summary>
        /// <param name="emitterTrackName">The emitter track name (in particle system asset).</param>
        /// <param name="paramName">The emitter parameter name (in particle emitter asset).</param>
        /// <param name="value">The value to set.</param>
        public void SetParameterValue(string emitterTrackName, string paramName, object value)
        {
            Internal_SetParameterValue(unmanagedPtr, emitterTrackName, paramName, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParameterValue(IntPtr obj, string emitterTrackName, string paramName, object value);

        /// <summary>
        /// Resets the particle system parameters to the default values from asset.
        /// </summary>
        public void ResetParameters()
        {
            Internal_ResetParameters(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ResetParameters(IntPtr obj);

        /// <summary>
        /// Resets the particles simulation state (clears the instance state data but preserves the instance parameters values).
        /// </summary>
        public void ResetSimulation()
        {
            Internal_ResetSimulation(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ResetSimulation(IntPtr obj);

        /// <summary>
        /// Performs the full particles simulation update (postponed for the next particle manager update).
        /// </summary>
        public void UpdateSimulation()
        {
            Internal_UpdateSimulation(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateSimulation(IntPtr obj);

        /// <summary>
        /// The particles simulation update modes.
        /// </summary>
        [Tooltip("The particles simulation update modes.")]
        public enum SimulationUpdateMode
        {
            /// <summary>
            /// Use realtime simulation updates. Updates particles during every game logic update.
            /// </summary>
            [Tooltip("Use realtime simulation updates. Updates particles during every game logic update.")]
            Realtime = 0,

            /// <summary>
            /// Use fixed timestep delta time to update particles simulation with a custom frequency.
            /// </summary>
            [Tooltip("Use fixed timestep delta time to update particles simulation with a custom frequency.")]
            FixedTimestep = 1,
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe partial struct ParameterOverride
        {
            public string Track;

            public Guid Id;

            public object Value;
        }
    }
}
