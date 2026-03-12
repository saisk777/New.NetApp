import { Component, inject, input, OnInit } from '@angular/core';
import { Member } from '../../_models/members';
import { NgClass, NgForOf, NgIf, NgStyle, DecimalPipe } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { environment } from '../../../environments/environment';
import { User } from '../../_models/user';
import { Photo } from '../../_models/photo';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [
    NgIf,
    NgForOf,
    NgClass,
    NgStyle,
    DecimalPipe,         // required for number pipe
    FileUploadModule
  ],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {
  private accountService = inject(AccountService);
  private membersService = inject(MembersService);
  member = input.required<Member>();
  uploder?: FileUploader;
  hasBaseDropZoneOver  = false;
  baseUrl = environment.apiUrl;
  user!: User;

  ngOnInit(): void {
    this.initializeUploder();
  }

  fileOverBase(e: any){
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo) {
    this.membersService.setMainPhoto(photo.id).subscribe({
      next: () => {
        const updatedMember = this.member();
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach(p => {
          p.isMain = p.id === photo.id;
        });
      }
    });
  }

  deletePhoto(photo: Photo) {
    this.membersService.deletePhoto(photo.id).subscribe({
      next: () => {
        const updatedMember = this.member();
        updatedMember.photos = updatedMember.photos.filter(p => p.id !== photo.id);
      }
    });
  }

  initializeUploder(){
    this.uploder = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
    });
    this.uploder.onBeforeUploadItem = (item) => {
      item.withCredentials = false;
    };
  }
}
