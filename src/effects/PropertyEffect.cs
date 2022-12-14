using Godot;
using System;
using System.Collections.Generic;

public class PropertyEffect : Node{
    

    protected bool can_stack = false;
    protected EffectsManager.PROPERTY_TYPE PropertyType;
    protected string effect_name;

    public virtual Vector2 ApplyEffect(Vector2 property){
        return property;
    }

    public virtual int ApplyEffect(int property){
        return property;
    }

    public virtual float ApplyEffect(float property){
        return property;
    }

    public EffectsManager.PROPERTY_TYPE GetPropertyType() => PropertyType;

    public bool CanStack() => can_stack;

    public string GetEffectName() => effect_name;

}
