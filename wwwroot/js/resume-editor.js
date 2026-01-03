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

function removeEduCard(button)
{
    button.closest('.education-card').remove();
    updateEduIndexes();
}

function addNewEduCard()
{
    var container = document.getElementById('education-container');

    var html = `<div class="container bg-light border rounded p-3 mb-3 education-card">

                        <div class="row justify-content-md-center">
                            <div class="col-md-4">
                                <label class="form-label small">Skola</label>
                                <input type="text" data-prop="SchoolName" name=""
                                class="form-control" placeholder="Skolnamn..." />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label small">Utbildning</label>
                                <input type="text" data-prop="DegreeName" name=""
                                class="form-control" placeholder="Utbildningsnamn..." />
                            </div>
                        </div>
                        <div class="row justify-content-md-center">
                            <div class="col-md-4">
                                <label class="form-label small">Startår</label>
                                <input type="text" data-prop="StartYear" name=""
                                class="form-control" placeholder="Startår..." />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label small">Slutår</label>
                                <input type="text" data-prop="EndYear" name=""
                                class="form-control" placeholder="Slutår..." />
                            </div>
                        </div>
                        <div class="row justify-content-md-center">
                            <div class="col-md-6">
                                <label class="form-label small">Beskrivning</label>
                                <textarea data-prop="Description" name="" class="form-control" rows="2" placeholder="Beskrivning..."></textarea>
                            </div>
                        </div>
                        <div class="rowjustify-content-md-center">
                            <div class="col-md-4">
                                <button type="button" class="btn btn-outline-danger"
                                onclick="removeEduCard(this)">X</button>
                            </div>
                        </div>

                    </div>`;

    container.insertAdjacentHTML('beforeend', html);
    updateEduIndexes();
}

function updateEduIndexes()
{
    var cards = document.querySelectorAll('.education-card');

    cards.forEach((card, index) => {
        var inputs = card.querySelectorAll('input, textarea');

        inputs.forEach(input => {
        
            var dataName = input.getAttribute('data-prop');

            if (dataName) {
                input.name = `Resume.EducationList[${index}].${dataName}`;
            }

        });

    });
}

function removeWorkCard(button)
{
    button.closest('.work-card').remove();
    updateWorkIndexes();
}

function addNewWorkCard()
{
    var container = document.getElementById('work-container');

    var html = `<div class="container bg-light border rounded p-3 mb-3 work-card">

                            <div class="row justify-content-md-center">
                                <div class="col-md-4">
                                    <label class="form-label small">Företag</label>
                                    <input type="text" data-prop="CompanyName" name=""
                                    class="form-control" placeholder="Företagsnamn..." />
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label small">Position</label>
                                    <input type="text" data-prop="Position" name=""
                                    class="form-control" placeholder="Position..." />
                                </div>
                            </div>
                            <div class="row justify-content-md-center">
                                <div class="col-md-4">
                                    <label class="form-label small">Start datum</label>
                                    <input type="date" data-prop="StartDate" name=""
                                    class="form-control" placeholder="Startdatum..." />
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label small">Slutår</label>
                                    <input type="date" data-prop="EndDate" name=""
                                    class="form-control" placeholder="Slutdatum..." />
                                </div>
                            </div>
                            <div class="row justify-content-md-center">
                                <div class="col-md-6">
                                    <label class="form-label small">Beskrivning</label>
                                    <textarea data-prop="Description" name="" class="form-control" rows="2" placeholder="Beskrivning..."></textarea>
                                </div>
                            </div>
                            <div class="rowjustify-content-md-center">
                                <div class="col-md-4">
                                    <button type="button" class="btn btn-outline-danger"
                                    onclick="removeWorkCard(this)">X</button>
                                </div>
                            </div>

                    </div>`;

    container.insertAdjacentHTML('beforeend', html);
    updateWorkIndexes();
}

function updateWorkIndexes()
{
    var cards = document.querySelectorAll('.work-card');

    cards.forEach((card, index) => {
        var inputs = card.querySelectorAll('input, textarea');

        inputs.forEach(input => {
        
            var dataName = input.getAttribute('data-prop');

            if (dataName) {
                input.name = `Resume.WorkList[${index}].${dataName}`;
            }

        });

    });
}