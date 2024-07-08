var total_program = 0;
var page = 1;
var pageSize = 100;
var lampiran = [];

$(document).ready(function () {
	ListProgram();
});


function ListProgram() {
	var params = {
		start: page,
		length: pageSize
	};
	RequestData('GET', '/v1/ProgressProgram/list_external', '#program_list', null, params, function (data) {
		if (data.succeeded) {
			if (page == 1)
				$('#program_list').html('');
			if (data.list.length > 0) {
				data.list.forEach(function (item) {
					var img = item.photo != null ? item.photo.resize : `${window.location.origin}/Content/images/no_image.jpg`;
					$('#program_list').append(`<div class="d-flex col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-1">
													<div class="card" >
														<img src="${img}" class="card-img-top">
														<div class="card-body">
	                                                       <h4 class="card-title mb-0 font-weight-bold">Nama Program :${item.jenisProgram.nama}</h4>
                                                          <h4 class="mt-1 card-title mb-0 font-weight-bold">Kegiatan : ${item.namaProgram}</h4>
	                                                     
														</div>
														<div class="card-footer border-0">
                                                             <span class="mt-2 d-flex justify-content-between">
																<a href="${window.location.origin}/Monitoring/DetailEksternal?id=${item.id}" class="card-link"><i></i> Detail</a>
                                                                <a data-detail='${JSON.stringify(item).replace(/' /g, " ")}' onclick="addProgress(this);" class="card-link"><i></i> Add Progress</a>
															</span>
														</div>
													</div>
												</div>`);
					total_program++;
				});
				if (data.count > total_program)
					$('#show_more_program').removeAttr('hidden');
				else
					$('#show_more_program').hide();
			}
		} else {
			ShowNotif(`${data.message} : ${data.description}`, "error");
		}
	})
}



function addProgress(el) {
	var data = $(el).data('detail');
	$('.clear').val('');
	$('#md-addProgress').modal('show');
	//$('#addProgress-title').html(data.namaProgram);
	$('#addProgress-body').html('');
	lampiran = [];
	data.items.forEach(function (item) {
		$('#addProgress-body').append(`
					<a class="card-header info" data-toggle="collapse" href="#${item.id}" aria-expanded="false" aria-controls="${item.id}">
						<div class="card-title lead collapsed">
							<label class="font-weight-bold mr-1">${item.kegiatan} (${item.detail?.progress ?? 0}%)</label>
						</div>
					</a>
					<div id="${item.id}" role="tabpanel" aria-labelledby="heading12" class="collapse" aria-expanded="false">
						<div class="card-content">
							<div class="card-body">
								<div class="row">
									<div class="col-md-6 mb-1">
										<label class="font-weight-bold ">Progres</label>
										<div>
                                            <input type="number" class="form-control" id="${item.id}_progress" oninput="maxNumber(this,100)" value="${item.detail?.progress ?? 0}" min="${item.detail?.progress ?? 0}" max="100">
										</div>
									</div>
									<div class="col-md-6 mb-1">
										<label class="font-weight-bold">Tanggal Progres</label>
										<div>
											<input id="${item.id}_tgl_progress" type="text" class="form-control datepicker" value="${item.detail?.tglProgress ? DateToStringFormat(item.detail?.tglProgress) : ""}">
										</div>
									</div>
									<div class="col-md-12 mb-1">
										<div>
											<label for="basicTextarea" class="cursor-pointer font-weight-bold">Deskripsi</label>
											<fieldset class="form-group">
											<textarea class="form-control" id="${item.id}_deskripsi" rows="2">${item.detail?.deskripsi ?? ""}</textarea>
										</fieldset>
										</div>
									</div>
									<div class="col-md-12 mb-1">
										<label class="font-weight-bold">Lampiran</label>
										<fieldset class="form-group">
												<div id="dz-${item.id}" class="dropzone" style="border-style: dashed;
														border-color: #16181e;">
													 <div class="dz-default dz-message">
														<img src="/Content/images/upload.png" class="text-center" width="10%">
														<p class="text-center">Upload Here</p>
													</div>
												</div>
										</fieldset>

									</div>
									 <div class="card-footer">
										<button class="btn btn-success btn-sm text-center" onclick="updateProgress('${item.id}');">Update Progress</button>
									</div>
								</div>
							</div>
						</div>
					</div>`);

		//Setfile(`#${item.id}_file`);
		var idProgress = item.id;
		SetDatePicker(`#${idProgress}_tgl_progress`, "DD MMMM, YYYY", null, null, true, item.detail?.tglProgress ? DateToStringFormat(item.detail?.tglProgress) : new Date());
		InitDropzone(`#dz-${idProgress}`, function () {
			if (item.detail?.lampiran) {
				$.each(item.detail.lampiran, function (i, item) {
					var file = {
						name: item.filename,
						size: 100,
						status: Dropzone.ADDED,
						accepted: true,
						id: item.id,
						isEdit: true
					};
					Dropzone.forElement(`#dz-${idProgress}`).emit("addedfile", file);
					Dropzone.forElement(`#dz-${idProgress}`).emit("complete", file);
				});
			}
        });
		
	});
}

function updateProgress(id) {
	ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
		if (!isConfirm) {
			return;
		}
		var param = {
			tglProgress: StringToDateFormat($(`#${id}_tgl_progress`).val(), "DD MMMM, YYYY"),
			deskripsi: $(`#${id}_deskripsi`).val(),
			idProgramItem: id,
			progress: $(`#${id}_progress`).val(),
			lampiran: lampiran
		}
		RequestData('POST', `/v1/ProgressProgram/add`, `#${id}`, null, JSON.stringify(param), function (data_obj) {
			if (data_obj.succeeded) {
				AlertMessage("Data Berhasil Disimpan", null, "success");
				$('#md-addProgress').modal('hide');
				lampiran = [];
				ListProgram();
			}
			else
				AlertMessage(data_obj.message);
		});
	});
}

//Dropzone
function InitDropzone(element, callback = null) {
	$(element).dropzone({
		addRemoveLinks: true,
		url: "/",
		success: async function (file, response) {
			let _base64;
			await FileToBase64(file)
				.then(dataBase64 => _base64 = dataBase64)
				.catch(error => {
					AlertMessage(error);
					return;
				});

			lampiran.push({
				filename: file.upload.filename,
				base64: _base64
			});

			file.previewElement.classList.add("dz-success");
		},
		error: function (file, response) {
			file.previewElement.classList.add("dz-error");
		},
		removedfile: function (file) {
			file.previewElement.remove();
			if (file.isEdit) {
				RemoveLampiran(file.id);
			}
		}
	});

	if (callback)
		return callback();
}

function RemoveLampiran(id) {
	ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
		if (isConfirm) {
			RequestData('DELETE', `/v1/ProgramMedia/delete/${id}`, '#md-Addlampiran', null, null, function (data_obj) {
				if (data_obj.succeeded) {
					AlertMessage("Data Berhasil Dihapus", null, "success");
				} else {
					AlertMessage(data_obj.message);
				}
			});
		}
	});
}

function addDetail(el) {
	var data = $(el).data('detail');
	console.log(data)
	$('.clear').val('');
	$('#md-addProgress').modal('show');
	$('#addProgress-title').html(data.namaProgram);
	$('#addProgress-body').html('');
	data.items.forEach(function (item) {
		$('#addProgress-body').append(`
					<a class="card-header info" data-toggle="collapse" href="#${item.id}" aria-expanded="false" aria-controls="${item.id}">
						<div class="card-title lead collapsed">
							<label class="font-weight-bold ">${item.kegiatan} (${item.progress}%)</label>
						</div>
					</a>
					<div id="${item.id}" role="tabpanel" aria-labelledby="heading12" class="collapse" aria-expanded="false">
						<div class="card-content">
							<div class="card-body">
								<div class="row">
									<div class="col-md-6 mb-1">
										<label class="font-weight-bold ">Progres</label>
										<div>
                                            <input type="number" class="form-control" id="${item.id}_progress" oninput="maxNumber(this,100)" value="${item.progress}" min="${item.progress}" max="100">
										</div>
									</div>
									<div class="col-md-6 mb-1">
										<label class="font-weight-bold">Tanggal Progres</label>
										<div>
											<input id="${item.id}_tgl_progress" type="text" class="form-control datepicker">
										</div>
									</div>
									<div class="col-md-12 mb-1">
										<div>
											<label for="basicTextarea" class="cursor-pointer font-weight-bold">Deskripsi</label>
											<fieldset class="form-group">
											<textarea class="form-control" id="${item.id}_deskripsi" rows="2">-</textarea>
										</fieldset>
										</div>
									</div>
									<div class="col-md-12 mb-1">
										<label class="font-weight-bold">Lampiran</label>
										<fieldset class="form-group">
												<div id="${item.id}_dz" class="dropzone" style="border-style: dashed;
														border-color: #16181e;">
													 <div class="dz-default dz-message">
														<img src="/Content/images/upload.png" class="text-center" width="10%">
														<p class="text-center">Upload Here</p>
													</div>
												</div>
										</fieldset>

									</div>
									 <div class="card-footer">
										<button class="btn btn-success btn-sm text-center" onclick="updateProgress('${item.id}');">Update Progress</button>
									</div>
								</div>
							</div>
						</div>
					</div>`
		);

	});
}