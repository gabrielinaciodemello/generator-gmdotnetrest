var Generator = require('yeoman-generator');

module.exports = class extends Generator {

    constructor(args, opts) {
        super(args, opts);
    }

    copyFiles(){
        var projectName = this.appname;
        
        this.fs.copyTpl(
            [this.templatePath('**'),this.templatePath('.*')],
            this.destinationPath(),
            { ProjectName: projectName }
        );
        
        this.fs.move(this.destinationPath('ProjectName.csproj'),this.destinationPath(`${projectName}.csproj`));
    }

};