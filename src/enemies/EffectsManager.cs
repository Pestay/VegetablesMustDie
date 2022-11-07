using Godot;
using System;
using System.Collections.Generic;

public class EffectsManager : Node2D{
    
    public enum PROPERTY_TYPE{ // Wich propertie modifies
        Velocity,
        Health,
    }

    Dictionary<PROPERTY_TYPE, List<PropertyEffect>> effects = new Dictionary<PROPERTY_TYPE, List<PropertyEffect>>(){
        {PROPERTY_TYPE.Velocity , new List<PropertyEffect>()},
        {PROPERTY_TYPE.Health , new List<PropertyEffect>()},

    }; 

    public override void _Ready(){
        // TO TEST
        //PackedScene sloweffect = GD.Load<PackedScene>("res://src/effects/SlowDownEffect.tscn");
        //AttachEffect(PROPERTY_TYPE.Velocity, sloweffect.Instance<SlowDownEffect>() );
    }

    public void AttachEffect(PROPERTY_TYPE type, PropertyEffect effect){
        AddChild(effect);
        effects[type].Add(effect);
    }


    public Vector2 ApplyEffectsForProperty(PROPERTY_TYPE type, Vector2 property){
        foreach( PropertyEffect effect in effects[type] ){
            property = effect.ApplyEffect(property);
        }
        return property;
    }


    public int ApplyEffectsForProperty(PROPERTY_TYPE type, int property){
        return property;
    }
    


}
