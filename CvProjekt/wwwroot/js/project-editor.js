function addNewProjCard(){
    var container = document.getElementById('project-container')

    var html = ` <div class="container bg-light border rounded p-3 mb-3 project-card">

                            <div class="row justify-content-md-center">
                                <div class="col-md-4">
                                    <label class="form-label small">Titel</label>
                                    <input type="text" data-prop="Title" name=""
                                    class="form-control" placeholder="Projektnamn..." />
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label small">Årtal</label>
                                    <input type="text" data-prop="Year" name=""
                                    class="form-control" placeholder="Årtal..." />
                                </div>
                            </div>
                            <div class="row justify-content-md-center">
                                <div class="col-md-6">
                                    <label class="form-label small">Språk</label>
                                    <input type="text" data-prop="Language" name=""
                                    class="form-control" placeholder="Språk..." />
                                </div>
                            </div>
                            <div class="row justify-content-md-center">
                                <div class="col-md-6">
                                    <label class="form-label small">Githublänk</label>
                                    <input type="text" data-prop="GithubLink" name=""
                                    class="form-control" placeholder="Githublänk..." />
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
                                    onclick="prepareDeleteProj(this)">X</button>
                                </div>
                            </div>
                        </div>`;

    container.insertAdjacentHTML('beforeend', html);
    updateProjIndexes();
}

function updateProjIndexes() {
    var cards = document.querySelectorAll('.project-card');

    cards.forEach((card, index) => {
        var inputs = card.querySelectorAll('input, textarea');

        inputs.forEach(input => {

            var dataName = input.getAttribute('data-prop');

            if (dataName) {
                input.name = `Projects[${index}].${dataName}`;
            }

        });

    });
}

let projToDelete = null;

function prepareDeleteProj(button) {
    projToDelete = button.closest('.project-card');
    
    const deletePopup = new bootstrap.Modal(document.getElementById('confirmDeletePopup'));
    deletePopup.show();
}

document.getElementById('confirmDeleteBtn').addEventListener('click', function() {
    if (projToDelete) {
        projToDelete.remove();
        updateProjIndexes();

        const deletePopupEl = document.getElementById('confirmDeletePopup');
        var popup = bootstrap.Modal.getInstance(deletePopupEl)
        popup.hide();

        projToDelete = null;
    }

});