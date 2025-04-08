import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css'
})
export class ServerErrorComponent {
  error: any;
  constructor(private router: Router){
    const navitagtion = this.router.getCurrentNavigation();
    this.error = navitagtion?.extras?.state?.['error'];
  }


}
