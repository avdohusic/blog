import { Injectable } from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {defaultToastOptions} from "@app/app.config";

@Injectable({providedIn: 'root'})
export class ToastService {

    constructor(
        private toaster: ToastrService
    ) {
    }

    success(message?: string): void {
        message = message ?? "Operation successfully";
        this.toaster.success(message, undefined, defaultToastOptions);
    }

    error(message?: string): void {
      message = message ?? "Operation error";
        this.toaster.error(message, undefined, defaultToastOptions);
    }

    info(message: string): void {
        this.toaster.info(message, undefined, defaultToastOptions);
    }

    warning(message: string): void {
        this.toaster.warning(message, undefined, defaultToastOptions);
    }
}
