using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = FlaxEngine.Assertions.Assert;


namespace FlaxEditor.Tests
{
    [TestClass]
    public class UndoTests
    {
        [Serializable]
        public class UndoObject
        {
            public int FieldInteger = 10;
            public float FieldFloat = 0.1f;
            public UndoObject FieldObject;

            public int PorpertyInteger { get; set; } = -10;
            public float PorpertyFloat { get; set; } = -0.1f;
            public UndoObject PorpertyObject { get; set; }

            public UndoObject()
            {
            }

            public UndoObject(bool addInstance)
            {
                if (!addInstance)
                {
                    return;
                }
                FieldObject = new UndoObject
                {
                    FieldInteger = 1,
                    FieldFloat = 1.1f,
                    FieldObject = new UndoObject(),
                    PorpertyFloat = 1.1f,
                    PorpertyInteger = 1,
                    PorpertyObject = null
                };
                PorpertyObject = new UndoObject
                {
                    FieldInteger = -1,
                    FieldFloat = -1.1f,
                    FieldObject = null,
                    PorpertyFloat = -1.1f,
                    PorpertyInteger = -1,
                    PorpertyObject = new UndoObject()
                };
            }
        }

        [TestMethod]
        public void UndoBasic()
        {
            var instance = new UndoObject(true);
            Undo.RecordBegin(instance, "Basic");
            instance.FieldFloat = 0;
            instance.FieldInteger = 0;
            instance.FieldObject = null;
            instance.PorpertyFloat = 0;
            instance.PorpertyInteger = 0;
            instance.PorpertyObject = null;
            Undo.RecordEnd();
            BasicUndoRedo(instance);

            instance = new UndoObject(true);
            Undo.RecordAction(instance, "Basic", () =>
            {
                instance.FieldFloat = 0;
                instance.FieldInteger = 0;
                instance.FieldObject = null;
                instance.PorpertyFloat = 0;
                instance.PorpertyInteger = 0;
                instance.PorpertyObject = null;
            });
            BasicUndoRedo(instance);

            object generic = new UndoObject(true);
            Undo.RecordAction(generic, "Basic", (i) =>
            {
                ((UndoObject)i).FieldFloat = 0;
                ((UndoObject)i).FieldInteger = 0;
                ((UndoObject)i).FieldObject = null;
                ((UndoObject)i).PorpertyFloat = 0;
                ((UndoObject)i).PorpertyInteger = 0;
                ((UndoObject)i).PorpertyObject = null;
            });
            BasicUndoRedo((UndoObject)generic);

            instance = new UndoObject(true);
            Undo.RecordAction(instance, "Basic", (i) =>
            {
                i.FieldFloat = 0;
                i.FieldInteger = 0;
                i.FieldObject = null;
                i.PorpertyFloat = 0;
                i.PorpertyInteger = 0;
                i.PorpertyObject = null;
            });
            BasicUndoRedo(instance);

            instance = new UndoObject(true);
            using(new Undo(instance, "Basic"))
            {
                instance.FieldFloat = 0;
                instance.FieldInteger = 0;
                instance.FieldObject = null;
                instance.PorpertyFloat = 0;
                instance.PorpertyInteger = 0;
                instance.PorpertyObject = null;
            }
            BasicUndoRedo(instance);
        }

        private static void BasicUndoRedo(UndoObject instance)
        {
            Undo.PerformUndo();
            Assert.AreEqual(10, instance.FieldInteger);
            Assert.AreEqual(0.1f, instance.FieldFloat);
            Assert.AreNotEqual(null, instance.FieldObject);
            Assert.AreEqual(-10, instance.PorpertyInteger);
            Assert.AreEqual(-0.1f, instance.PorpertyFloat);
            Assert.AreNotEqual(null, instance.PorpertyObject);
            Undo.PerformRedo();
            Assert.AreEqual(0, instance.FieldInteger);
            Assert.AreEqual(0, instance.FieldFloat);
            Assert.AreEqual(null, instance.FieldObject);
            Assert.AreEqual(0, instance.PorpertyInteger);
            Assert.AreEqual(0, instance.PorpertyFloat);
            Assert.AreEqual(null, instance.PorpertyObject);
        }
    }
}