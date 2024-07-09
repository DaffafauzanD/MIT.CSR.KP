var total_program = 0;
var page = 1;
var pageSize = 3;
var _status = "all";

$(document).ready(function () {
	ListBanner();
	ListCompany();
	GetRekap();
	ListProgram();
});

function OnChangeStatus(status) {
	$(".program_status").removeClass("badge badge-info");
	$("#program_status_label-" + status).addClass("badge badge-info");
	_status = status;
	page = 1;
	ListProgram();

}

function ListBanner() {
    RequestData('GET', '/v1/Banner/list', '#carousel-interval', '#carousel-interval', null, function (data) {
		if (data.succeeded) {
			console.log(data)
            if (data.list.length > 0) {
                var count = 0;
                $('#carousel-interval').html('');
                var list = ``;
                var img_list = ``;
                var prev_next = `<a class="carousel-control-prev" href="#carousel-interval" role="button" data-slide="prev">
					                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
					                <span class="sr-only">Previous</span>
				                </a>
				                <a class="carousel-control-next" href="#carousel-interval" role="button" data-slide="next">
					                <span class="carousel-control-next-icon" aria-hidden="true"></span>
					                <span class="sr-only">Next</span>
				                </a>`;
				
				data.list.forEach(function (item) {
					var active = '';
					if (count == 0)
						active = 'active';
					list += `<li data-target="#carousel-interval" data-slide-to="${count}" class="${active}"></li>`
					img_list += `<div class="carousel-item ${active}">
									<img src="${item.url.original}" class="w-100 col" height="459px">
								</div>`
					count++;
				});
				$('#carousel-interval').html(`
					<ol class="carousel-indicators">${list}</ol>
					<div class="carousel-inner" role="listbox">
					${img_list}
					</div>
					${prev_next}
				`);
            } else {
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    })
}

function GetRekap() {
	RequestData('GET', '/v1/Report/rekap_home', '#rekap_program', null, null, function (data) {
		if (data.succeeded) {
			$('#rekap_program-on_progress').html(`${formatNumber(data.data.onProgress) } Kegiatan`);
			$('#rekap_program-on_progress_rupiah').html(`Rp. ${formatNumber(data.data.onProgressRupiah)}`);
			$('#rekap_program-done').html(`${formatNumber(data.data.done)} Kegiatan`);
			$('#rekap_program-done_rupiah').html(`Rp. ${formatNumber(data.data.doneRupiah) }`);
		} else {
			ShowNotif(`${data.message} : ${data.description}`, "error");
		}
	})
}

function ListCompany() {
	RequestData('GET', '/v1/report/top_company', '#top_company_body', '#top_company_body', null, function (data) {
		if (data.succeeded) {
			$('#top_company_body').html('');
			if (data.list.length > 0) {
				data.list.forEach(function (item) {
					var img = item.logo != null ? item.logo.resize : `${window.location.origin}/Content/images/no_image.jpg`;
					$('#top_company_body').append(`<div class="media">
												<a class="media-left" href="#">
													<img src="${img}" alt="Generic placeholder image" height="64" width="64">
												</a>
												<div class="media-body">
													<h4 class="media-heading">${item.company.namaPerusahaan}</h4>
													Rp. ${formatNumber(item.rupiah)}
												</div>
											</div>`);
				});
			} else {
			}
		} else {
			ShowNotif(`${data.message} : ${data.description}`, "error");
		}
	})
}

function ListProgram() {
	var status = "0";
	if (_status == "done")
		status = "4";
	else if (_status == "on_progress")
		status = "3";
    var param = {
        filter: [
            {
                field: "status",
				search: status
            }
        ],
        sort: {
            field: "startTglPelaksanaan",
            type: 2
        },
        start: page,
        length: pageSize
    }
	RequestData('POST', '/v1/Program/list', '#program_list', null, JSON.stringify(param), function (data) {
		if (data.succeeded) {
			if (page == 1)
				$('#program_list').html('');
            if (data.list.length > 0) {
				data.list.forEach(function (item) {
					var img = item.photo != null ? item.photo.resize : `${window.location.origin}/Content/images/no_image.jpg`;
					$('#program_list').append(`<div class="d-flex col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-1">
													<div class="card">
														<img src="${img}" class="card-img-top">
														<div class="card-body">
															<h4 class="card-title mb-0 font-weight-bold">${item.namaProgram.nama}</h4>
															<h5 class="mt-2 card-text font-weight-bold">Jenis Program : ${item.jenisProgram.nama}</h5>
															<p class="card-text">${item.deskripsi}</p>
													
														</div>
	                                                      <div class="card-footer border-0">
                                                            <span class="mt-2 d-flex justify-content-between">
																<p class="text-muted mb-0">${item.lokasiDati}</p>
																<a href="${window.location.origin}/Home/Detail/${item.id}" class="card-link">Read More</a>
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
				page++;
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    })
}