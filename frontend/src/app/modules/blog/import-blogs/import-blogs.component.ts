import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {BlogApiService} from "@app/_services/blog.api.service";
import {Router} from "@angular/router";
import {ToastService} from "@app/_services/toast.service";

@Component({
  selector: 'app-import-blogs',
  templateUrl: './import-blogs.component.html'
})
export class ImportBlogsComponent implements OnInit {
  loading = false;
  uploadForm = new FormGroup({
    file: new FormControl('', [Validators.required]),
    fileSource: new FormControl('', [Validators.required])
  });

  constructor(
    private blogApiService: BlogApiService,
    private router: Router,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
  }

  get f(){
    return this.uploadForm.controls;
  }


  onFileChange(event:any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.uploadForm?.patchValue({
        fileSource: file
      });
    }
  }

  submit(){
    let formObject = this.uploadForm.get('fileSource')?.value;
    if(formObject) {
      const formData = new FormData();
      formData.append('file', formObject);
      this.loading = true;
      this.blogApiService.uploadBlogs(formData).subscribe((x) => {
        this.loading = false;
        this.toastService.success("Blog created");
        this.goToIndex();
      });
    }
  }

  goToIndex() {
    this.router.navigate([`/home`]);
  }
}
