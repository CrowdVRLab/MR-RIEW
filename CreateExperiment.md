HOW TO CREATE A WORKFLOW

- Copy the trial scenes and add your content to them
- Create a Experiment Scriptable Object with right-click context menu (you can edit a name for the experiment)
- in Build Settings the first scene need to be Start_e (or a  modified copy of it)
- in Build Settings all the involved scenes need to be inserted in the scene list.
- Add trials (experiment conditions) by clicking the plus in Inpsector Panel of Experiment Scriptable Object
   - Name is the name of the trial
   - Scene Name needs to match to an existing scene
   - Questionnaire Ref Name needs to exist in DataToCollect Scriptable Object

- Create DataToCollect Scriptable Object with right-click context menu (you can edit a name )
    - Add Parts by clicking plus
       - each part of a questionnaire contains:
          - name (name of that part)
          - reference name (that is the ref name used by Experiment Scriptable Object)
          - description (optional)
          - SubParts Scripts that are Questionnaire Scriptable Object
              - you can add the questionnaire increasing the array and by dragging the scriptable objects 
              
              
In Experiment Manager a pre questionnaire experiment and post questionnaire experiment can be linked, and need to match the reference
name of the DataToCollect Scriptable Object.


- Add the data collect to the firebase manager and the ui builder script