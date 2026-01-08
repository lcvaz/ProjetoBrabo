import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

/**
 * Header component for the marketplace application
 * Provides navigation, search, and quick access to cart and profile
 */
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  /**
   * Number of items in the shopping cart
   * Displays a badge when greater than 0
   */
  @Input() cartItemCount: number = 0;

  /**
   * Event emitted when the search action is triggered
   * Emits the current search query
   */
  @Output() search = new EventEmitter<string>();

  /**
   * Event emitted when the cart button is clicked
   */
  @Output() cartClick = new EventEmitter<void>();

  /**
   * Event emitted when the profile button is clicked
   */
  @Output() profileClick = new EventEmitter<void>();

  /**
   * Current search query value
   */
  searchQuery: string = '';

  /**
   * Handles the search action
   * Emits the search event with the current query
   */
  onSearch(): void {
    if (this.searchQuery.trim()) {
      this.search.emit(this.searchQuery.trim());
    }
  }

  /**
   * Handles cart button click
   * Emits the cartClick event
   */
  onCartClick(): void {
    this.cartClick.emit();
  }

  /**
   * Handles profile button click
   * Emits the profileClick event
   */
  onProfileClick(): void {
    this.profileClick.emit();
  }
}
