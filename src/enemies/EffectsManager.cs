using Godot;
using System;
using System.Collections.Generic;

public class EffectsManager : Node2D{
    
    public enum PROPERTY_TYPE{ // Wich propertie modifies
        Velocity,
        Health,
    }

    Dictionary<PROPERTY_TYPE, List<PropertyEffect>> current_effects = new Dictionary<PROPERTY_TYPE, List<PropertyEffect>>(){
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
        current_effects[type].Add(effect);
    }


    public void CreateNewPropertyEffect(PackedScene effect_scene){
        PropertyEffect new_effect = effect_scene.Instance<PropertyEffect>();
        EffectsManager.PROPERTY_TYPE property = new_effect.GetPropertyType();

        if(!new_effect.CanStack()){ // Check if there are other effects
            foreach (PropertyEffect effect in current_effects[property]){
                if(effect.GetEffectName() == new_effect.GetEffectName()){
                    new_effect.QueueFree();
                    return;
                }
            }
        }
        
        AddChild(new_effect);
        current_effects[property].Add(new_effect);
    }


    public Vector2 ApplyEffectsForProperty(PROPERTY_TYPE type, Vector2 property){
        foreach( PropertyEffect effect in current_effects[type] ){
            property = effect.ApplyEffect(property);
        }
        return property;
    }


    public int ApplyEffectsForProperty(PROPERTY_TYPE type, int property){
        return property;
    }
    


}
