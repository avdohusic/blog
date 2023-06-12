import { Component, OnInit } from '@angular/core';
import {editorConfig} from "@app/app.config";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {BlogApiService} from "@app/_services/blog.api.service";
import {ActivatedRoute, Router} from "@angular/router";
import {Blog} from "@app/_models";
import {ToastService} from "@app/_services/toast.service";

@Component({
  selector: 'app-update-blog',
  templateUrl: './update-blog.component.html'
})
export class UpdateBlogComponent implements OnInit {
  config = editorConfig;
  loading = false;
  blogId: string | undefined;
  blogForm = new FormGroup({
    blogId: new FormControl('', [Validators.required]),
    title: new FormControl('', [Validators.required]),
    author: new FormControl('', [Validators.required]),
    content: new FormControl('', [Validators.required])
  });
  constructor(
    private activeRoute: ActivatedRoute,
    private blogApiService: BlogApiService,
    private router: Router,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    this.blogId = this.activeRoute.snapshot?.params['blogId'];
    this.blogApiService.getById(this.blogId).subscribe((blog) => {
      this.blogForm.patchValue(blog);
    });
  }

  get f(){
    return this.blogForm?.controls;
  }

  submit(): void  {
    if(this.blogForm.valid) {
      this.loading = true;
      this.blogApiService.updateBlog(this.blogId, this.blogForm.value as Blog).subscribe(x => {
        this.loading = false;
        this.toastService.success("Blog updated");
        this.goToIndex();
      });
    }
  }

  deleteBlog(): void {
    this.loading = true;
    this.blogApiService.deleteBlog(this.blogId).subscribe(x => {
      this.loading = false;
      this.toastService.success("Blog deleted");
      this.goToIndex();
    });
  }

  goToIndex(): void {
    this.router.navigate([`/home`]);
  }
}
