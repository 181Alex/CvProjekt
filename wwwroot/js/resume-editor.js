function removeQualifRow(button)
{
    button.closest('.input-group').remove();

    updateQualifIndexes();
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
                <button type="button" class="btn btn-outline-danger" onclick="removeQualifRow(this)">X</button>
            </div>
        `;

    container.insertAdjacentHTML('beforeend', html);
    updateQualifIndexes();
}

function updateQualifIndexes()
{
    var container = document.getElementById('qualifications-container');
    var rows = container.querySelectorAll('.qualification-row');

    rows.forEach((row, index) => {
        var input = row.querySelector('input');
        input.name = `Resume.Qualifications[${index}].Name`;
    });
}