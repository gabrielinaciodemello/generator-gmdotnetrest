var Generator = require('yeoman-generator');

module.exports = class extends Generator {

    constructor(args, opts) {
        super(args, opts);
        this.argument('JsonPath', { type: String, required: true });
    }

    createModel(){
        var projectName = this.appname;
        var json = this.fs.readJSON(this.options.JsonPath);
        json.forEach((model) => {
            var modelName = model.Name;
            var properties = '';
            model.Properties.forEach((prop) => {
                properties += `public ${prop.Type} ${prop.Name} { get; set; } \r\n\t\t`; 
            });
            this.fs.copyTpl(
                this.templatePath('Model.cs'),
                this.destinationPath(`Models/${modelName}.cs`),
                { ProjectName: this.appname, Properties: properties, ModelName: modelName }
            );    
        });
    }

    createController(){
        var projectName = this.appname;
        var json = this.fs.readJSON(this.options.JsonPath);
        json.forEach((model) => {
            var modelName = model.Name;
            this.fs.copyTpl(
                this.templatePath('Controller.cs'),
                this.destinationPath(`Controllers/${modelName}sController.cs`),
                { ProjectName: this.appname, ModelName: modelName }
            );
        });
    }

    createValidator(){
        var projectName = this.appname;
        var json = this.fs.readJSON(this.options.JsonPath);
        json.forEach((model) => {
            var modelName = model.Name;
            //Rules logic
            var rules = '';
            model.Properties.forEach((prop)=>{
                var line = '';
                if(prop.Required && prop.Required.length === 2 && prop.Required[0] === true)
                    line += `.NotEmpty().WithMessage("${prop.Required[1]}")`;
                if(prop.Length && prop.Length.length === 3)
                    line += `.Length(${prop.Length[0]},${prop.Length[1]}).WithMessage("${prop.Length[2]}")`;
                if(prop.IsEmail && prop.IsEmail.length === 2 && prop.IsEmail[0] === true)
                    line += `.EmailAddress().WithMessage("${prop.IsEmail[1]}")`;
                if(line !== ''){
                    line = `RuleFor(e => e.${prop.Name})${line};\r\n\t\t\t`;
                    rules += line;
                }
            });
            //Unique logic
            var unique = null;
            if(model.Unique && model.Unique.length === 2){
                var uniqueFields = [];
                model.Unique[0].forEach((field)=>{
                    uniqueFields.push(`x.${field} == entity.${field}`);
                })
                var where = uniqueFields.join(' && ');
                unique = { Where: where, Conditional: where.replace(/x/g,"result"), Message: model.Unique[1] };
            }

            this.fs.copyTpl(
                this.templatePath('Validator.cs'),
                this.destinationPath(`Validators/${modelName}Validator.cs`),
                { ProjectName: this.appname, ModelName: modelName, Rules: rules, Unique: unique }
            );
        });
    }

};