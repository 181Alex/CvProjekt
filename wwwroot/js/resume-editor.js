function removeQualifRow(button)
{
    button.closest('.input-group').remove();
}

function addNewQualifRow()
{
    var container = document.getElementById('qualifications-container');

    var index = container.children.length;

    var html = `
            <div class="input-group mb-2 qualification-row">
                <input type="text" 
                       name="Resume.Qualifications[${index}].Name" 
                       class="form-control" 
                       placeholder="Ny kompetens..." />
                <button type="button" class="btn btn-outline-danger" onclick="removeRow(this)">X</button>
            </div>
        `;

    container.insertAdjacentHTML('beforeend', html);
}