import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IImageListModel } from '../../../Models/Profile/IImageListModel';
import { ProfileService } from '../../../services/Profile/profile.service';

@Component({
  selector: 'app-image-collection',
  templateUrl: './image-collection.component.html',
  styleUrls: ['./image-collection.component.css'],
  standalone: false
})
export class ImageCollectionComponent {

  imageCollection : IImageListModel = <IImageListModel>{};
  currentImageURL : string = "";

  constructor(private profileService : ProfileService, private router: Router) {
    this.imageCollection.images = [];
  }

  ngOnInit(){
    this.profileService.getProfileImages()
    .subscribe({next: (data: any)=>{
      this.imageCollection = data;
    }, error :  (err)=> {

    }})
  }

  addImageToList() {
    debugger;
    // https://drive.google.com/file/d/1fSH2GhZ6VFLtiGDMmGqx2fg0Sg4GIo4q/view?usp=sharing
    if (this.currentImageURL.startsWith("https://drive.google.com")){
    debugger;
      var candidateID = this.currentImageURL.split("https://drive.google.com/file/d/")[1]
      var fileID = candidateID.split("/")[0];
      this.currentImageURL = "https://lh3.googleusercontent.com/d/" + fileID
    }
    this.imageCollection.images.unshift(this.currentImageURL);
    this.currentImageURL = "";
  }

  saveAllImages() {
    this.profileService.addImagesToProfile(this.imageCollection)
    .subscribe({next: ()=>{
      this.router.navigate(['/profile'])
    }, error : (err) =>{
    }})
  }

  deleteImage(index: number){
    this.imageCollection.images.splice(index, 1);
  }
}
