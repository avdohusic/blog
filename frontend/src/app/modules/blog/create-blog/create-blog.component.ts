import { Component, OnInit } from '@angular/core';
import {Blog} from "@app/_models";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {BlogApiService} from "@app/_services/blog.api.service";
import {Router} from "@angular/router";
import {editorConfig} from "@app/app.config";
import {ToastService} from "@app/_services/toast.service";

@Component({
  selector: 'app-create-blog',
  templateUrl: './create-blog.component.html'
})
export class CreateBlogComponent implements OnInit {
  config = editorConfig;
  loading = false;
  blogForm = new FormGroup({
    title: new FormControl('', [Validators.required]),
    author: new FormControl('', [Validators.required]),
    content: new FormControl('', [Validators.required])
  });
  constructor(
    private blogApiService: BlogApiService,
    private router: Router,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
  }

  get f(){
    return this.blogForm?.controls;
  }

  submit() {
    if(this.blogForm.valid) {
      this.loading = true;
      this.blogApiService.createBlog(this.blogForm.value as Blog).subscribe(x => {
        this.loading = false;
        if(x){
          this.toastService.success("Blog created");
          this.goToIndex();
        }
      });
    }
  }

  goToIndex() {
    this.router.navigate([`/home`]);
  }
}
