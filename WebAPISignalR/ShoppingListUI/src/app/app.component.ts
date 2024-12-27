import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ShoppingListComponent } from '../components/shopping-list/shopping-list.component';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ShoppingListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'ShoppingListUI';
}
