// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Storage for a collection of fungus variables that can then be accessed globally.
    /// </summary>
    public class GlobalVariables : MonoBehaviour
    {
        private Flowchart holder;

        Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        void Awake()
        {
            holder = new GameObject("GlobalVariables").AddComponent<Flowchart>();
            holder.transform.parent = transform;
        }

		public Variable GetVariable(string variableKey)
		{
			Variable v = null;
			variables.TryGetValue(variableKey, out v);
			return v;
		}

        public VariableBase<T> GetOrAddVariable<T>(string variableKey, T defaultvalue, Type type)
        {
            Variable v = null;
            VariableBase<T> vAsT = null;
            var res = variables.TryGetValue(variableKey, out v);

            if(res && v != null)
            {
                vAsT = v as VariableBase<T>;

                if (vAsT != null)
                {
                    return vAsT;
                }
                else
                {
                    Debug.LogError("A fungus variable of name " + variableKey + " already exists, but of a different type");
                }
            }
            else
            {
                //create the variable
                vAsT = holder.gameObject.AddComponent(type) as VariableBase<T>;
                vAsT.Value = defaultvalue;
                vAsT.Key = variableKey;
                vAsT.Scope = VariableScope.Public;
                variables[variableKey] = vAsT;
                holder.Variables.Add(vAsT);
            }

            return vAsT;
        }

		public FungusSaver.SaveObj StoreVariables() {
			var data = new FungusSaver.SaveObj();

			data.boolVars = new List<BoolVar>();
			data.intVars = new List<IntVar>();
			data.floatVars = new List<FloatVar>();
			data.stringVars = new List<StringVar>();

			foreach (var pair in variables) {
				var v = pair.Value;

				// Save string
				var stringVariable = v as StringVariable;
				if (stringVariable != null) {
					var d = new StringVar();
					d.Key = stringVariable.Key;
					d.Value = stringVariable.Value;
					data.stringVars.Add(d);
				}

				// Save int
				var intVariable = v as IntegerVariable;
				if (intVariable != null) {
					var d = new IntVar();
					d.Key = intVariable.Key;
					d.Value = intVariable.Value;
					data.intVars.Add(d);
				}

				// Save float
				var floatVariable = v as FloatVariable;
				if (floatVariable != null) {
					var d = new FloatVar();
					d.Key = floatVariable.Key;
					d.Value = floatVariable.Value;
					data.floatVars.Add(d);
				}

				// Save bool
				var boolVariable = v as BooleanVariable;
				if (boolVariable != null) {
					var d = new BoolVar();
					d.Key = boolVariable.Key;
					d.Value = boolVariable.Value;
					data.boolVars.Add(d);
				}
			}

			return data;
		} 
    }
}